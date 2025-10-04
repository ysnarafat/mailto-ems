using ClosedXML.Excel;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Enums;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public class ContactExportService : IContactExportService
    {
        private IContactExportUnitOfWork _contactExportUnitOfWork;
        private IContactUnitOfWork _contactUnitOfWork;
        private IGroupUnitOfWork _groupUnitOfWork;
        public ContactExportService(IContactExportUnitOfWork contactExportUnitOfWork, IContactUnitOfWork contactUnitOfWork, IGroupUnitOfWork groupUnitOfWork)
        {
            _contactExportUnitOfWork = contactExportUnitOfWork;
            _contactUnitOfWork = contactUnitOfWork;
            _groupUnitOfWork = groupUnitOfWork;
        }

        public async Task<(IList<DownloadQueue> Items, int Total, int TotalFilter)> GetAllContactExportFileFromDownloadQueueAsync(
          Guid? userId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DownloadQueue, object>>>()
            {
                ["Created"] = v => v.Created
            };
            var result = (await _contactExportUnitOfWork.DownloadQueueRepository.GetAsync(x => x,
                                                  x => !x.IsDeleted && x.IsActive &&
                                                  (!userId.HasValue || x.UserId == userId.Value) &&
                                                  x.FileName.Contains(searchText) &&
                                                  (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport),
                                                  x => x.ApplyOrdering(columnsMap, orderBy),
                                                  x => x.Include(y => y.DownloadQueueSubEntities),
                                                  pageIndex, pageSize,
                                                  true));

            if (result.Items == null) throw new NotFoundException(nameof(DownloadQueue), userId);

            result.Total = await _contactExportUnitOfWork.DownloadQueueRepository.GetCountAsync(x => x.UserId == userId && (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport));

            return (result.Items, result.Total, result.TotalFilter);

        }
        public async Task<IList<Contact>> GetAllContactAsync(Guid? userId)
        {
            var contacts = await _contactUnitOfWork.ContactRepository.GetAsync<Contact>(
                x => x, x => (!userId.HasValue || x.UserId == userId.Value), null, x => x.Include(i => i.ContactValueMaps).ThenInclude(i => i.FieldMap)
                .Include(i => i.ContactGroups).ThenInclude(i => i.Group), true
                );
            return contacts;
        }

        public async Task<IList<ContactGroup>> GetAllContactGroupByUserIdAsync(Guid? userId, int groupId)
        {
            var contacts = await _contactUnitOfWork.GroupContactRepository.GetAsync<ContactGroup>(
                x => x, x => ((x.GroupId == groupId) && (x.Contact.UserId == userId)), null, x => x.Include(i => i.Contact).ThenInclude(i => i.ContactValueMaps).ThenInclude(i => i.FieldMap).Include(i => i.Group), true
                );
            return contacts;
        }
        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            var contact = await _contactUnitOfWork.ContactRepository.GetByIdAsync(contactId);
            return contact;
        }


        public async Task<IList<(int Value, string Text, int Count)>> GetAllGroupAsync(Guid? userId)
        {
            return (await _groupUnitOfWork.GroupRepository.GetAsync(x => new { Value = x.Id, Text = x.Name, Count = x.ContactGroups.Count() },
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.UserId == userId.Value), x => x.OrderBy(o => o.Name), null, true))
                                                   .Select(x => (Value: x.Value, Text: x.Text, Count: x.Count)).ToList();
        }

        public async Task SaveDownloadQueueAsync(DownloadQueue downloadQueue)
        {
            await _contactExportUnitOfWork.DownloadQueueRepository.AddAsync(downloadQueue);
            await _contactExportUnitOfWork.SaveChangesAsync();
        }
        public async Task AddDownloadQueueSubEntitiesAsync(IList<DownloadQueueSubEntity> downloadQueueSubEntities)
        {
            await _contactExportUnitOfWork.DownloadQueueSubEntityRepository.AddRangeAsync(downloadQueueSubEntities);
            await _contactExportUnitOfWork.SaveChangesAsync();
        }
        public async Task<IList<DownloadQueue>> GetDownloadQueueAsync()
        {
            var result = await _contactExportUnitOfWork.DownloadQueueRepository.GetAsync(
                x => x,
                x => (x.IsProcessing == true || x.IsSucceed == false) && (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport),
                null,
                x => x.Include(x => x.DownloadQueueSubEntities),
                true);
            return result;
        }

        public async Task<DownloadQueue> GetDownloadQueueByIdAsync(int downloadQueueId)
        {
            var downloadQueue = await _contactExportUnitOfWork.DownloadQueueRepository.GetByIdAsync(downloadQueueId);

            return downloadQueue;
        }
        public async Task UpdateDownloadQueueAsync(DownloadQueue downloadQueue)
        {
            await _contactExportUnitOfWork.DownloadQueueRepository.UpdateAsync(downloadQueue);
            await _contactExportUnitOfWork.SaveChangesAsync();
        }

        public async Task ExcelExportForAllContactsAsync(DownloadQueue downloadQueue)
        {
            using (var workbook = new XLWorkbook())
            {
                var contacts = await GetAllContactAsync(downloadQueue.UserId);

                var worksheet = workbook.Worksheets.Add("All Contacts");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Contact Email Address";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 2).Value = "Groups";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                var fieldmaplist = contacts.SelectMany(x => x.ContactValueMaps).Select(x => x.FieldMap.DisplayName).Distinct().ToList();

                int currentColumn = 3, columnCount = 2;
                Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

                for (int j = 0; j < fieldmaplist.Count(); j++)
                {
                    worksheet.Cell(currentRow, j + currentColumn).Value = fieldmaplist[j];
                    worksheet.Cell(currentRow, j + currentColumn).Style.Font.Bold = true;
                    keyValuePairs.Add(fieldmaplist[j], j + currentColumn);
                    columnCount++;
                }


                foreach (var item in contacts)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Email;

                    string group = string.Join(", ", item.ContactGroups.Select(x => x.Group.Name));


                    worksheet.Cell(currentRow, 2).Value = group;

                    for (int j = 0; j < item.ContactValueMaps.Count(); j++)
                    {
                        var key = item.ContactValueMaps.Select(x => x.FieldMap.DisplayName).ToArray()[j];
                        if (keyValuePairs.ContainsKey(key))
                        {
                            worksheet.Cell(currentRow, keyValuePairs[key]).Value = item.ContactValueMaps[j].Value;

                        }

                    }
                }

                worksheet.Columns("1", columnCount.ToString()).AdjustToContents();

                var memory = new MemoryStream();
                using (var stream = new FileStream(downloadQueue.FileUrl, FileMode.Create))
                {
                    workbook.SaveAs(stream);
                }
            }
        }
        public async Task ExcelExportForGroupwiseContactsAsync(DownloadQueue downloadQueue)
        {
            using (var workbook = new XLWorkbook())
            {
                for (int cnt = 0; cnt < downloadQueue.DownloadQueueSubEntities.Count(); cnt++)
                {
                    int currentRow = 1, currentColumn = 3, columnCount = 2;
                    var contacts = await GetAllContactGroupByUserIdAsync(downloadQueue.UserId, downloadQueue.DownloadQueueSubEntities[cnt].DownloadQueueSubEntityId);
                    var groupName = contacts.Select(x => x.Group.Name).FirstOrDefault();
                    var worksheet = workbook.Worksheets.Add(groupName + " contacts");
                    worksheet.Cell(currentRow, 1).Value = "Contact Email Address";
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 2).Value = "Group";
                    worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                    var contactsForSelectedGroupId = contacts.Select(x => x.Contact).ToList();
                    var fieldmaplist = contacts.Select(x => x.Contact).SelectMany(x => x.ContactValueMaps).Select(x => x.FieldMap.DisplayName).Distinct().ToList();

                    Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

                    for (int j = 0; j < fieldmaplist.Count(); j++)
                    {
                        worksheet.Cell(currentRow, j + currentColumn).Value = fieldmaplist[j];

                        worksheet.Cell(currentRow, j + currentColumn).Style.Font.Bold = true;
                        keyValuePairs.Add(fieldmaplist[j], j + currentColumn);
                        columnCount++;
                    }
                    foreach (var item in contactsForSelectedGroupId)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = item.Email;
                        string group = string.Join(", ", item.ContactGroups.Select(x => x.Group.Name));

                        worksheet.Cell(currentRow, 2).Value = group;

                        for (int j = 0; j < item.ContactValueMaps.Count(); j++)
                        {
                            var key = item.ContactValueMaps.Select(x => x.FieldMap.DisplayName).ToArray()[j];
                            if (keyValuePairs.ContainsKey(key))
                            {
                                worksheet.Cell(currentRow, keyValuePairs[key]).Value = item.ContactValueMaps[j].Value;
                            }
                        }
                    }

                    worksheet.Columns("1", columnCount.ToString()).AdjustToContents();

                }
                using (var stream = new FileStream(downloadQueue.FileUrl, FileMode.Create))
                {
                    workbook.SaveAs(stream);
                }
            }
        }
        public void Dispose()
        {
            _contactUnitOfWork.Dispose();
        }

    }
}
