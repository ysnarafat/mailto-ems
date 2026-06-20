using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Constants;
using EmailMarketing.Shared.Infrastructure.Extensions;
using EmailMarketing.Shared.Abstractions.Services;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Modules.Contacts.UnitOfWorks;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Modules.Contacts.Services;

public class ContactUploadService : IContactUploadService
{
    private IContactUploadUnitOfWork _contactUploadUnitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ContactUploadService(
        IContactUploadUnitOfWork contactUploadUnitOfWork,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _contactUploadUnitOfWork = contactUploadUnitOfWork;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(ContactUpload contactUpload)
    {
        if (string.IsNullOrWhiteSpace(contactUpload.FileUrl) || !File.Exists(contactUpload.FileUrl))
            throw new Exception("File not found.");

        var newContacts = new List<Contact>();
        var existingContacts = new List<Contact>();
        var isFirstRowHeader = true;
        var existCount = 0;
        var invalidCount = 0;
        var emailIndex = contactUpload.ContactUploadFieldMaps
            .FirstOrDefault(x => x.FieldMap.DisplayName == ConstantsValue.ContactFieldMapEmail)?.Index;

        if (!emailIndex.HasValue) throw new Exception("Email column not found.");

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

                        if (contactUpload.IsUpdateExisting)
                        {
                            var existingContact = await _contactUploadUnitOfWork.ContactRepository.GetFirstOrDefaultAsync(
                                x => x, x => x.UserId == contactUpload.UserId && x.Email.ToLower() == newContact.Email,
                                null, true);

                            if (existingContact != null)
                            {
                                existingContact = await GenerateUpdateContact(contactUpload, emailIndex.Value, existingContact, reader);
                                existingContacts.Add(existingContact);
                                existCount++;
                                continue;
                            }
                        }

                        newContacts.Add(newContact);
                    }
                } while (reader.NextResult());
            }
        }

        if (newContacts.Any())
            await _contactUploadUnitOfWork.ContactRepository.AddRangeAsync(newContacts);
        if (existingContacts.Any())
            await _contactUploadUnitOfWork.ContactRepository.UpdateRangeAsync(existingContacts);
        await _contactUploadUnitOfWork.SaveChangesAsync();

        await ContactUploadSucceedUpdate(contactUpload.Id, newContacts.Count);

        return (newContacts.Count, existingContacts.Count, invalidCount);
    }

    private async Task ContactUploadSucceedUpdate(int contactUploadId, int newContactCount)
    {
        var existingContactUpload = await _contactUploadUnitOfWork.ContactUploadRepository.GetFirstOrDefaultAsync(
            x => x, x => x.Id == contactUploadId, null, true);
        existingContactUpload.IsSucceed = true;
        existingContactUpload.IsProcessing = false;
        existingContactUpload.SucceedEntryCount = newContactCount;
        await _contactUploadUnitOfWork.ContactUploadRepository.UpdateAsync(existingContactUpload);
        await _contactUploadUnitOfWork.SaveChangesAsync();
    }

    private async Task<Contact> GenerateNewContact(ContactUpload contactUpload, int emailIndex, IExcelDataReader reader)
    {
        var newContact = new Contact
        {
            ContactValueMaps = new List<ContactValueMap>(),
            ContactGroups = new List<ContactGroup>(),
            ContactUploadId = contactUpload.Id,
            Email = reader.GetString(emailIndex),
            UserId = contactUpload.UserId,
            Created = _dateTime.Now,
            CreatedBy = _currentUserService.UserId
        };

        for (int i = 0; i < contactUpload.ContactUploadFieldMaps.Count; i++)
        {
            var fileIndex = contactUpload.ContactUploadFieldMaps[i].Index;
            if (fileIndex == emailIndex) continue;
            newContact.ContactValueMaps.Add(new ContactValueMap
            {
                FieldMapId = contactUpload.ContactUploadFieldMaps[i].FieldMapId,
                Value = reader.GetValue(fileIndex).ParseObjectToString()
            });
        }

        for (int i = 0; i < contactUpload.ContactUploadGroups.Count; i++)
        {
            newContact.ContactGroups.Add(new ContactGroup
            {
                GroupId = contactUpload.ContactUploadGroups[i].GroupId
            });
        }

        return newContact;
    }

    private async Task<Contact> GenerateUpdateContact(ContactUpload contactUpload, int emailIndex, Contact existingContact, IExcelDataReader reader)
    {
        existingContact.ContactValueMaps = new List<ContactValueMap>();
        existingContact.ContactGroups = new List<ContactGroup>();
        var newContactValMaps = new List<ContactValueMap>();
        var newContactGroups = new List<ContactGroup>();

        for (int i = 0; i < contactUpload.ContactUploadFieldMaps.Count; i++)
        {
            var fileIndex = contactUpload.ContactUploadFieldMaps[i].Index;
            if (fileIndex == emailIndex) continue;
            newContactValMaps.Add(new ContactValueMap
            {
                ContactId = existingContact.Id,
                FieldMapId = contactUpload.ContactUploadFieldMaps[i].FieldMapId,
                Value = reader.GetValue(fileIndex).ParseObjectToString()
            });
        }

        for (int i = 0; i < contactUpload.ContactUploadGroups.Count; i++)
        {
            newContactGroups.Add(new ContactGroup
            {
                GroupId = contactUpload.ContactUploadGroups[i].GroupId,
                ContactId = existingContact.Id
            });
        }

        var existingContactValueMaps = await _contactUploadUnitOfWork.ContactValueMapRepository.GetAsync(
            x => x, x => x.ContactId == existingContact.Id, null, null, true);
        if (existingContactValueMaps.Any())
            await _contactUploadUnitOfWork.ContactValueMapRepository.DeleteRangeAsync(existingContactValueMaps);
        if (newContactValMaps.Any())
            await _contactUploadUnitOfWork.ContactValueMapRepository.AddRangeAsync(newContactValMaps);
        await _contactUploadUnitOfWork.SaveChangesAsync();

        var existingContactGroups = await _contactUploadUnitOfWork.ContactGroupRepository.GetAsync(
            x => x, x => x.ContactId == existingContact.Id, null, null, true);
        if (existingContactGroups.Any())
            await _contactUploadUnitOfWork.ContactGroupRepository.DeleteRangeAsync(existingContactGroups);
        if (newContactGroups.Any())
            await _contactUploadUnitOfWork.ContactGroupRepository.AddRangeAsync(newContactGroups);
        await _contactUploadUnitOfWork.SaveChangesAsync();

        existingContact.ContactUploadId = contactUpload.Id;
        existingContact.LastModified = _dateTime.Now;
        existingContact.LastModifiedBy = _currentUserService.UserId;

        return existingContact;
    }

    public async Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(int contactUploadId)
    {
        var contactUpload = await _contactUploadUnitOfWork.ContactUploadRepository.GetFirstOrDefaultAsync(
            x => x, x => x.Id == contactUploadId,
            x => x.Include(i => i.ContactUploadFieldMaps).ThenInclude(i => i.FieldMap).Include(i => i.ContactUploadGroups),
            true);

        return await ContactExcelImportAsync(contactUpload);
    }

    public async Task<IList<ContactUpload>> GetUploadedContact()
    {
        return await _contactUploadUnitOfWork.ContactUploadRepository.GetAsync(
            x => x, x => x.IsProcessing == true, null, null, true);
    }

    public async Task<IList<(int Value, string Text, bool IsStandard)>> GetAllFieldMapForSelectAsync(Guid? userId)
    {
        return (await _contactUploadUnitOfWork.FieldMapRepository.GetAsync(
            x => new { Value = x.Id, Text = x.DisplayName, IsStandard = x.IsStandard },
            x => !x.IsDeleted && x.IsActive && (x.IsStandard || (!userId.HasValue || x.UserId == userId.Value)),
            x => x.OrderBy(o => o.DisplayName), null, true))
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
        return await _contactUploadUnitOfWork.FieldMapRepository.IsExistsAsync(
            x => values.Contains(x.Id) && x.DisplayName == ConstantsValue.ContactFieldMapEmail);
    }

    public async Task<(IList<ContactUpload> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
    {
        var columnsMap = new Dictionary<string, Expression<Func<ContactUpload, object>>>()
        {
            ["FileName"] = v => v.FileName,
            ["Created"] = v => v.Created
        };

        var result = await _contactUploadUnitOfWork.ContactUploadRepository.GetAsync(
            x => x, x => (!userId.HasValue || x.UserId == userId.Value) && x.FileName.Contains(searchText),
            x => x.ApplyOrdering(columnsMap, orderBy), null,
            pageIndex, pageSize, true);

        result.Total = await _contactUploadUnitOfWork.ContactUploadRepository.GetCountAsync(x => x.UserId == userId);

        return (result.Items, result.Total, result.TotalFilter);
    }

    public async Task<ContactUpload> GetByIdAsync(int id)
    {
        return await _contactUploadUnitOfWork.ContactUploadRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(ContactUpload entity)
    {
        entity.IsProcessing = entity.IsProcessing == true ? false : true;
        await _contactUploadUnitOfWork.ContactUploadRepository.UpdateAsync(entity);
        await _contactUploadUnitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        _contactUploadUnitOfWork?.Dispose();
    }
}



