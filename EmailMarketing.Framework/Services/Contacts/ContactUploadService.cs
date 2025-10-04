using EmailMarketing.Common.Constants;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public class ContactUploadService : IContactUploadService
    {
        private IContactUploadUnitOfWork _contactUploadUnitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ContactUploadService(IContactUploadUnitOfWork contactUploadUnitOfWork, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _contactUploadUnitOfWork = contactUploadUnitOfWork;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        #region Import
        public async Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(ContactUpload contactUpload)
        {
            if (string.IsNullOrWhiteSpace(contactUpload.FileUrl) || !File.Exists(contactUpload.FileUrl)) throw new Exception("File not found.");

            var newContacts = new List<Contact>();
            var existingContacts = new List<Contact>();
            var isFirstRowHeader = true;
            var existCount = 0;
            var invalidCount = 0;
            var emailIndex = contactUpload.ContactUploadFieldMaps.FirstOrDefault(x => x.FieldMap.DisplayName == ConstantsValue.ContactFieldMapEmail)?.Index;

            if(!emailIndex.HasValue) throw new Exception("Email column not found.");

            using (var stream = File.Open(contactUpload.FileUrl, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            if (isFirstRowHeader && contactUpload.HasColumnHeader)
                            {
                                isFirstRowHeader = false;
                                continue;
                            }

                            var newContact = await GenerateNewContact(contactUpload, emailIndex.Value, reader);

                            if (string.IsNullOrWhiteSpace(newContact.Email))
                            {
                                invalidCount++;
                                continue;
                            }

                            #region Existing Contact Update
                            if (contactUpload.IsUpdateExisting)
                            {
                                var existingContact = await _contactUploadUnitOfWork.ContactRepository.GetFirstOrDefaultAsync(x => x,
                                                                x => x.UserId == contactUpload.UserId && x.Email.ToLower() == newContact.Email,
                                                                null, true);

                                if (existingContact != null)
                                {
                                    existingContact = await this.GenerateUpdateContact(contactUpload, emailIndex.Value, existingContact, reader);

                                    existingContacts.Add(existingContact);

                                    existCount++;
                                    continue;
                                }
                            }
                            #endregion

                            newContacts.Add(newContact);
                        }
                    } while (reader.NextResult());
                }
            }

            if(newContacts.Any())
                await _contactUploadUnitOfWork.ContactRepository.AddRangeAsync(newContacts);
            if(existingContacts.Any())
                await _contactUploadUnitOfWork.ContactRepository.UpdateRangeAsync(existingContacts);
            await _contactUploadUnitOfWork.SaveChangesAsync();

            #region Contact Upload Update
            await this.ContactUploadSucceedUpdate(contactUpload.Id, newContacts.Count);
            #endregion

            return (newContacts.Count, existingContacts.Count, invalidCount);
        }

        private async Task ContactUploadSucceedUpdate(int contactUploadId, int newContactCount)
        {
            var existingContactUpload = await _contactUploadUnitOfWork.ContactUploadRepository.GetFirstOrDefaultAsync(x => x,
                                                   x => x.Id == contactUploadId, null, true);
            existingContactUpload.IsSucceed = true;
            existingContactUpload.IsProcessing = false;
            existingContactUpload.SucceedEntryCount = newContactCount;
            await _contactUploadUnitOfWork.ContactUploadRepository.UpdateAsync(existingContactUpload);
            await _contactUploadUnitOfWork.SaveChangesAsync();
        }

        private async Task<Contact> GenerateNewContact(ContactUpload contactUpload, int emailIndex, IExcelDataReader reader)
        {
            var newContact = new Contact();
            newContact.ContactValueMaps = new List<ContactValueMap>();
            newContact.ContactGroups = new List<ContactGroup>();
            newContact.ContactUploadId = contactUpload.Id;
            newContact.Email = reader.GetString(emailIndex);
            newContact.UserId = contactUpload.UserId;
            newContact.Created = _dateTime.Now;
            newContact.CreatedBy = _currentUserService.UserId;

            #region Contact Value Maps Add
            for (int i = 0; i < contactUpload.ContactUploadFieldMaps.Count; i++)
            {
                var fileIndex = contactUpload.ContactUploadFieldMaps[i].Index;
                if (fileIndex == emailIndex) continue;
                var contactValMap = new ContactValueMap();
                contactValMap.FieldMapId = contactUpload.ContactUploadFieldMaps[i].FieldMapId;
                contactValMap.Value = reader.GetValue(fileIndex).ParseObjectToString();
                newContact.ContactValueMaps.Add(contactValMap);
            }
            #endregion

            #region Contact Groups Add
            for (int i = 0; i < contactUpload.ContactUploadGroups.Count; i++)
            {
                var contactGroup = new ContactGroup();
                contactGroup.GroupId = contactUpload.ContactUploadGroups[i].GroupId;
                newContact.ContactGroups.Add(contactGroup);
            }
            #endregion

            return newContact;
        }

        private  async Task<Contact> GenerateUpdateContact(ContactUpload contactUpload, int emailIndex, Contact existingContact, IExcelDataReader reader)
        {
            existingContact.ContactValueMaps = new List<ContactValueMap>();
            existingContact.ContactGroups = new List<ContactGroup>();
            var newContactValMaps = new List<ContactValueMap>();
            var newContactGroups = new List<ContactGroup>();

            for (int i = 0; i < contactUpload.ContactUploadFieldMaps.Count; i++)
            {
                var fileIndex = contactUpload.ContactUploadFieldMaps[i].Index;
                if (fileIndex == emailIndex) continue;
                var contactValMap = new ContactValueMap();
                contactValMap.ContactId = existingContact.Id;
                contactValMap.FieldMapId = contactUpload.ContactUploadFieldMaps[i].FieldMapId;
                contactValMap.Value = reader.GetValue(fileIndex).ParseObjectToString();
                newContactValMaps.Add(contactValMap);
            }

            for (int i = 0; i < contactUpload.ContactUploadGroups.Count; i++)
            {
                var contactGroup = new ContactGroup();
                contactGroup.GroupId = contactUpload.ContactUploadGroups[i].GroupId;
                contactGroup.ContactId = existingContact.Id;
                newContactGroups.Add(contactGroup);
            }

            #region Contact Value Maps Update
            var existingContactValueMaps = await _contactUploadUnitOfWork.ContactValueMapRepository.GetAsync(x => x,
                                                    x => x.ContactId == existingContact.Id, null, null, true);
            if (existingContactValueMaps.Any())
                await _contactUploadUnitOfWork.ContactValueMapRepository.DeleteRangeAsync(existingContactValueMaps);
            if (newContactValMaps.Any())
                await _contactUploadUnitOfWork.ContactValueMapRepository.AddRangeAsync(newContactValMaps);
            await _contactUploadUnitOfWork.SaveChangesAsync();
            #endregion

            #region Contact Groups Update
            var existingContactGroups = await _contactUploadUnitOfWork.ContactGroupRepository.GetAsync(x => x,
                                                    x => x.ContactId == existingContact.Id, null, null, true);
            if (existingContactGroups.Any())
                await _contactUploadUnitOfWork.ContactGroupRepository.DeleteRangeAsync(existingContactGroups);
            if (newContactGroups.Any())
                await _contactUploadUnitOfWork.ContactGroupRepository.AddRangeAsync(newContactGroups);
            await _contactUploadUnitOfWork.SaveChangesAsync();
            #endregion

            existingContact.ContactUploadId = contactUpload.Id;
            existingContact.LastModified = _dateTime.Now;
            existingContact.LastModifiedBy = _currentUserService.UserId;

            return existingContact;
        }

        public async Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(int contactUploadId)
        {
            var contactUpload = await _contactUploadUnitOfWork.ContactUploadRepository.GetFirstOrDefaultAsync(x => x, x => x.Id == contactUploadId, 
                                    x => x.Include(i => i.ContactUploadFieldMaps).ThenInclude(i => i.FieldMap).Include(i => i.ContactUploadGroups), true);

            var result = await this.ContactExcelImportAsync(contactUpload);

            return (result.SucceedCount, result.ExistCount, result.InvalidCount);
        }

        public async Task<IList<ContactUpload>> GetUploadedContact()
        {
            var result = await _contactUploadUnitOfWork.ContactUploadRepository.GetAsync(x => x, x => x.IsProcessing == true, null, null, true);
            return result;
        }
        #endregion

        #region Upload
        public async Task<IList<(int Value, string Text, bool IsStandard)>> GetAllFieldMapForSelectAsync(Guid? userId)
        {
            return (await _contactUploadUnitOfWork.FieldMapRepository.GetAsync(x => new { Value= x.Id, Text= x.DisplayName, IsStandard= x.IsStandard }, 
                                                    x => !x.IsDeleted && x.IsActive &&
                                                    (x.IsStandard || (!userId.HasValue || x.UserId == userId.Value)), x => x.OrderBy(o => o.DisplayName), null, true))
                                                    .Select(x => (Value: x.Value, Text: x.Text, IsStandard: x.IsStandard)).ToList();
        }

        public async Task AddContactUploadAsync(ContactUpload entity)
        {
            entity.IsProcessing = true;
            entity.IsSucceed = false;
            entity.Created = _dateTime.Now;
            entity.CreatedBy = _currentUserService.UserId;
            
            await _contactUploadUnitOfWork.ContactUploadRepository.AddAsync(entity);
            await _contactUploadUnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsSelectedEmailFieldMap(IList<int> values)
        {
            return await _contactUploadUnitOfWork.FieldMapRepository.IsExistsAsync(x => values.Contains(x.Id) && x.DisplayName == ConstantsValue.ContactFieldMapEmail);
        }
        #endregion

        public async Task<(IList<ContactUpload> Items, int Total, int TotalFilter)> GetAllAsync(
           Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<ContactUpload, object>>>()
            {
                ["FileName"] = v => v.FileName,
                ["Created"] = v => v.Created
            };

            var result = await _contactUploadUnitOfWork.ContactUploadRepository.GetAsync(x => x, 
                x => (!userId.HasValue || x.UserId == userId.Value) && x.FileName.Contains(searchText),
                x => x.ApplyOrdering(columnsMap, orderBy), null,
                pageIndex, pageSize, true);

            result.Total = await _contactUploadUnitOfWork.ContactUploadRepository.GetCountAsync(x => x.UserId == userId);

            return (result.Items, result.Total, result.TotalFilter);
        }

        //public async Task<(IList<Contact> Items, int Total, int TotalFilter)> GetContactAsync(
        //   Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
        //{
        //    var columnsMap = new Dictionary<string, Expression<Func<Contact, object>>>()
        //    {
        //        ["Email"] = v => v.Email,
        //        //["Group"] = v => v.Gr
        //    };

        //    var result = await _contactUploadUnitOfWork.ContactUploadRepository.GetAsync(x => x,
        //        x => (!userId.HasValue || x.UserId == userId.Value) && x.FileName.Contains(searchText),
        //        x => x.ApplyOrdering(columnsMap, orderBy), x => x.Include(i => i.ContactUploadGroups).ThenInclude(i => i.Group),
        //        pageIndex, pageSize, true);

        //    result.Total = await _contactUploadUnitOfWork.ContactUploadRepository.GetCountAsync(x => x.UserId == userId);

        //    return (result.Items, result.Total, result.TotalFilter);
        //}
        public async Task<ContactUpload> GetByIdAsync(int id)
        {
           return await _contactUploadUnitOfWork.ContactUploadRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ContactUpload entity)
        {
            entity.IsProcessing = entity.IsProcessing == true? false : true ;

            await _contactUploadUnitOfWork.ContactUploadRepository.UpdateAsync(entity);
            await _contactUploadUnitOfWork.SaveChangesAsync();
        }
        public void Dispose()
        {
            _contactUploadUnitOfWork?.Dispose();
        }
    }
}
