using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
using EmailMarketing.Shared.Infrastructure.Exceptions;
using EmailMarketing.Shared.Infrastructure.Extensions;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing.Enums;
using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Modules.Contacts.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Modules.Contacts.Services;

public class ContactExportService : IContactExportService
{
    private IContactExportUnitOfWork _contactExportUnitOfWork;
    private IContactUnitOfWork _contactUnitOfWork;
    private IGroupReadRepository _groupReadRepository;

    public ContactExportService(
        IContactExportUnitOfWork contactExportUnitOfWork,
        IContactUnitOfWork contactUnitOfWork,
        IGroupReadRepository groupReadRepository)
    {
        _contactExportUnitOfWork = contactExportUnitOfWork;
        _contactUnitOfWork = contactUnitOfWork;
        _groupReadRepository = groupReadRepository;
    }

    public async Task<(IList<DownloadQueue> Items, int Total, int TotalFilter)> GetAllContactExportFileFromDownloadQueueAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
    {
        var columnsMap = new Dictionary<string, Expression<Func<DownloadQueue, object>>>()
        {
            ["Created"] = v => v.Created
        };

        var result = await _contactExportUnitOfWork.DownloadQueueRepository.GetAsync(
            x => x,
            x => !x.IsDeleted && x.IsActive &&
                 (!userId.HasValue || x.UserId == userId.Value) &&
                 x.FileName.Contains(searchText) &&
                 (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport),
            x => x.ApplyOrdering(columnsMap, orderBy),
            x => x.Include(y => y.DownloadQueueSubEntities),
            pageIndex, pageSize, true);

        if (result.Items == null) throw new NotFoundException(nameof(DownloadQueue), userId);

        result.Total = await _contactExportUnitOfWork.DownloadQueueRepository.GetCountAsync(
            x => x.UserId == userId &&
                 (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport));

        return (result.Items, result.Total, result.TotalFilter);
    }

    public async Task<IList<Contact>> GetAllContactAsync(Guid? userId)
    {
        return await _contactUnitOfWork.ContactRepository.GetAsync<Contact>(
            x => x, x => (!userId.HasValue || x.UserId == userId.Value), null,
            x => x.Include(i => i.ContactValueMaps).ThenInclude(i => i.FieldMap)
                   .Include(i => i.ContactGroups).ThenInclude(i => i.Group),
            true);
    }

    public async Task<IList<ContactGroup>> GetAllContactGroupByUserIdAsync(Guid? userId, int groupId)
    {
        return await _contactUnitOfWork.GroupContactRepository.GetAsync<ContactGroup>(
            x => x, x => (x.GroupId == groupId) && (x.Contact.UserId == userId), null,
            x => x.Include(i => i.Contact).ThenInclude(i => i.ContactValueMaps).ThenInclude(i => i.FieldMap)
                   .Include(i => i.Group),
            true);
    }

    public async Task<Contact> GetContactByIdAsync(int contactId)
    {
        return await _contactUnitOfWork.ContactRepository.GetByIdAsync(contactId);
    }

    public async Task<IList<(int Value, string Text, int Count)>> GetAllGroupAsync(Guid? userId)
    {
        return await _groupReadRepository.GetGroupsForSelectAsync(userId);
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
        return await _contactExportUnitOfWork.DownloadQueueRepository.GetAsync(
            x => x,
            x => (x.IsProcessing == true || x.IsSucceed == false) &&
                 (x.DownloadQueueFor == DownloadQueueFor.ContactAllExport || x.DownloadQueueFor == DownloadQueueFor.ContactGroupWiseExport),
            null,
            x => x.Include(y => y.DownloadQueueSubEntities),
            true);
    }

    public async Task<DownloadQueue> GetDownloadQueueByIdAsync(int downloadQueueId)
    {
        return await _contactExportUnitOfWork.DownloadQueueRepository.GetByIdAsync(downloadQueueId);
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

            var fieldmaplist = contacts.SelectMany(x => x.ContactValueMaps)
                .Select(x => x.FieldMap.DisplayName).Distinct().ToList();

            int currentColumn = 3, columnCount = 2;
            var keyValuePairs = new Dictionary<string, int>();

            for (int j = 0; j < fieldmaplist.Count; j++)
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
                worksheet.Cell(currentRow, 2).Value = string.Join(", ", item.ContactGroups.Select(x => x.Group.Name));

                for (int j = 0; j < item.ContactValueMaps.Count; j++)
                {
                    var key = item.ContactValueMaps.Select(x => x.FieldMap.DisplayName).ToArray()[j];
                    if (keyValuePairs.ContainsKey(key))
                        worksheet.Cell(currentRow, keyValuePairs[key]).Value = item.ContactValueMaps[j].Value;
                }
            }

            worksheet.Columns("1", columnCount.ToString()).AdjustToContents();

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
            for (int cnt = 0; cnt < downloadQueue.DownloadQueueSubEntities.Count; cnt++)
            {
                int currentRow = 1, currentColumn = 3, columnCount = 2;
                var contacts = await GetAllContactGroupByUserIdAsync(
                    downloadQueue.UserId, downloadQueue.DownloadQueueSubEntities[cnt].DownloadQueueSubEntityId);
                var groupName = contacts.Select(x => x.Group.Name).FirstOrDefault();
                var worksheet = workbook.Worksheets.Add(groupName + " contacts");

                worksheet.Cell(currentRow, 1).Value = "Contact Email Address";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Value = "Group";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;

                var contactsForSelectedGroupId = contacts.Select(x => x.Contact).ToList();
                var fieldmaplist = contactsForSelectedGroupId
                    .SelectMany(x => x.ContactValueMaps).Select(x => x.FieldMap.DisplayName).Distinct().ToList();

                var keyValuePairs = new Dictionary<string, int>();

                for (int j = 0; j < fieldmaplist.Count; j++)
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
                    worksheet.Cell(currentRow, 2).Value = string.Join(", ", item.ContactGroups.Select(x => x.Group.Name));

                    for (int j = 0; j < item.ContactValueMaps.Count; j++)
                    {
                        var key = item.ContactValueMaps.Select(x => x.FieldMap.DisplayName).ToArray()[j];
                        if (keyValuePairs.ContainsKey(key))
                            worksheet.Cell(currentRow, keyValuePairs[key]).Value = item.ContactValueMaps[j].Value;
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
