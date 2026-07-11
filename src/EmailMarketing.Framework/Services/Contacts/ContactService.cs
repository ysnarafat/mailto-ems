
using EmailMarketing.Common.Constants;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public class ContactService : IContactService
    {
        private IContactUnitOfWork _contactUnitOfWork;

        private IGroupUnitOfWork _groupUnitOfWork;


        public ContactService(IContactUnitOfWork contactUnitOfWork, IGroupUnitOfWork groupUnitOfWork)
        {
            _contactUnitOfWork = contactUnitOfWork;
            _groupUnitOfWork = groupUnitOfWork;
        }

        public async Task<(IList<Contact> Items, int Total, int TotalFilter)> GetAllContactAsync(
            Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<Entities.Contacts.Contact, object>>>()
            {
                ["Group"] = v => v.Email,
                ["Email"] = v => v.Email
            };

            var result = await _contactUnitOfWork.ContactRepository.GetAsync<Entities.Contacts.Contact>(
                x => x, x => (!userId.HasValue || x.UserId == userId.Value) && (x.Email.Contains(searchText)),
                x => x.ApplyOrdering(columnsMap, orderBy), x => x.Include(i => i.ContactGroups).ThenInclude(i => i.Group),
                pageIndex, pageSize, true);

            result.Total = await _contactUnitOfWork.ContactRepository.GetCountAsync(x => x.UserId == userId);

            return (result.Items, result.Total, result.TotalFilter);
        }

        public async Task<(IList<Contact> Items, int Total, int TotalFilter)> GetContactByContactUploadIdAsync(Guid? userId, int contactUploadId,
        string searchText,
        string orderBy,
        int pageIndex,
        int pageSize)
        {
            var columnsMap = new Dictionary<string, Expression<Func<Entities.Contacts.Contact, object>>>()
            {
                ["Group"] = v => v.Email,
                ["Email"] = v => v.Email
            };
            var result = await _contactUnitOfWork.ContactRepository.GetAsync(
                x => x, x => !x.IsDeleted && x.IsActive &&
                (!userId.HasValue || x.UserId == userId.Value) && (x.ContactUploadId == contactUploadId),
                   x => x.ApplyOrdering(columnsMap, orderBy), x => x.Include(i => i.ContactGroups).ThenInclude(i => i.Group),
                pageIndex, pageSize, true);
            return result;
        }
        public async Task<Contact> GetByIdAsync(int id)
        {
            var result = await _contactUnitOfWork.ContactRepository.GetFirstOrDefaultAsync(
                x => x, x => x.Id == id,
                x => x.Include(i => i.ContactGroups).ThenInclude(i => i.Group)
                        .Include(i => i.ContactValueMaps).ThenInclude(i => i.FieldMap), true);

            if (result == null) throw new NotFoundException(nameof(Contact), id);

            return result;
        }
        public async Task<Contact> DeleteAsync(int id)
        {
            var contact = await GetByIdAsync(id);
            if (contact == null) throw new NotFoundException(nameof(Contact), id);
            await _contactUnitOfWork.ContactRepository.DeleteAsync(id);
            await _contactUnitOfWork.SaveChangesAsync();
            return contact;
        }

        public async Task<int> GroupContactCountAsync(int id)
        {
            return await _contactUnitOfWork.GroupContactRepository.GetCountAsync(x => x.ContactId == id);
        }

        public async Task AddContact(Contact contact)
        {
            await _contactUnitOfWork.ContactRepository.AddAsync(contact);
            await _contactUnitOfWork.SaveChangesAsync();
        }

        public async Task AddContacValueMaps(IList<ContactValueMap> contactValueMaps)
        {
            await _contactUnitOfWork.ContactValueMapRepository.AddRangeAsync(contactValueMaps);
            await _contactUnitOfWork.SaveChangesAsync();
        }
        public async Task AddContactGroups(IList<ContactGroup> contactGroups)
        {
            await _contactUnitOfWork.GroupContactRepository.AddRangeAsync(contactGroups);
            await _contactUnitOfWork.SaveChangesAsync();
        }
        public async Task UpdateAsync(Contact contact)
        {
            var isExists = await _contactUnitOfWork.ContactRepository.IsExistsAsync(x => x.Email == contact.Email && x.Id != contact.Id && x.UserId == contact.UserId);
            if (isExists)
                throw new DuplicationException(nameof(contact.Email));

            var updateEntity = await _contactUnitOfWork.ContactRepository.GetFirstOrDefaultAsync(x => x, x => x.Id == contact.Id,
                null, false);

            var existingContactGroups = (await _contactUnitOfWork.GroupContactRepository.GetAsync(x => x,
                x => x.ContactId == updateEntity.Id, null, null, false)) ?? new List<ContactGroup>();
            if (existingContactGroups.Any())
                await _contactUnitOfWork.GroupContactRepository.DeleteRangeAsync(existingContactGroups);

            var existingContactValueMaps = (await _contactUnitOfWork.ContactValueMapRepository.GetAsync(x => x,
                x => x.ContactId == updateEntity.Id, null, null, false)) ?? new List<ContactValueMap>();
            if (existingContactValueMaps.Any())
                await _contactUnitOfWork.ContactValueMapRepository.DeleteRangeAsync(existingContactValueMaps);

            updateEntity.Email = contact.Email;
            updateEntity.LastModified = contact.LastModified;
            updateEntity.LastModifiedBy = contact.LastModifiedBy;
            await _contactUnitOfWork.ContactRepository.UpdateAsync(updateEntity);

            var contactValueMaps = new List<ContactValueMap>();
            foreach (var item in contact.ContactValueMaps ?? new List<ContactValueMap>())
            {
                contactValueMaps.Add(new ContactValueMap
                {
                    Value = item.Value,
                    FieldMapId = item.FieldMapId,
                    ContactId = contact.Id
                });
            }

            var contactGroups = new List<ContactGroup>();
            foreach (var item in contact.ContactGroups ?? new List<ContactGroup>())
            {
                contactGroups.Add(new ContactGroup
                {
                    ContactId = contact.Id,
                    GroupId = item.GroupId
                });
            }

            if (contactGroups.Any())
                await _contactUnitOfWork.GroupContactRepository.AddRangeAsync(contactGroups);

            if (contactValueMaps.Any())
                await _contactUnitOfWork.ContactValueMapRepository.AddRangeAsync(contactValueMaps);

            await _contactUnitOfWork.SaveChangesAsync();
        }
        public async Task<IList<(int Value, string Text, int Count)>> GetAllGroupsAsync(Guid? userId)
        {
            return (await _groupUnitOfWork.GroupRepository.GetAsync(x => new ValueTuple<int, string, int>(x.Id, x.Name, x.ContactGroups.Count),
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.UserId == userId.Value), x => x.OrderBy(o => o.Name), x => x.Include(i => i.ContactGroups), true));
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            var result = await _groupUnitOfWork.GroupRepository.GetByIdAsync(id);
            return result;
        }
        public async Task DeleteContactGroupAsync(int contactId)
        {
            var contactGroup = await _contactUnitOfWork.GroupContactRepository.GetAsync(
                x => x, x => x.ContactId == contactId, null, null, true);
            if (contactGroup == null) throw new NotFoundException(nameof(ContactGroup), contactId);

            await _contactUnitOfWork.GroupContactRepository.DeleteRangeAsync(contactGroup);
            await _contactUnitOfWork.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(IList<ContactValueMap> contactValueMaps)
        {
            await _contactUnitOfWork.ContactValueMapRepository.DeleteRangeAsync(contactValueMaps);
            await _contactUnitOfWork.SaveChangesAsync();
        }

        public async Task<ContactValueMap> GetContactValueMapByIdAsync(int id)
        {
            var result = await _contactUnitOfWork.ContactValueMapRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<IList<(int Value, string Text)>> GetAllContactValueMapsStandard()
        {
            return (await _contactUnitOfWork.FieldMapRepository.GetAsync(x => new ValueTuple<int, string>(x.Id, x.DisplayName),
                                                   x => !x.IsDeleted && x.IsActive &&
                                                    x.IsStandard == true && x.DisplayName != ConstantsValue.ContactFieldMapEmail, null, null, true));
        }

        public async Task<IList<(int Id, int Value, string Text, string Input)>> GetAllSelectedContactValueMapsStandard(int contactId)
        {
            return (await _contactUnitOfWork.ContactValueMapRepository.GetAsync(x => new ValueTuple<int, int, string, string>(x.Id, x.FieldMap.Id, x.FieldMap.DisplayName, x.Value),
                                                   x => !x.IsDeleted && x.IsActive && x.FieldMap.IsStandard == true &&
                                                   x.FieldMap.DisplayName != ConstantsValue.ContactFieldMapEmail &&
                                                   x.ContactId == contactId,
                                                   null, null, true));
        }

        public async Task<IList<(int Value, string Text)>> GetAllContactValueMapsCustom(Guid? userId)
        {
            return (await _contactUnitOfWork.FieldMapRepository.GetAsync(x => new ValueTuple<int, string>(x.Id, x.DisplayName),
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.UserId == userId.Value) && x.IsStandard == false,
                                                   null, null, true));
        }
        public async Task<IList<(int Id, int Value, string Text, string Input)>> GetAllContactValueMapsCustom(Guid? userId, int contactId)
        {
            return (await _contactUnitOfWork.ContactValueMapRepository.GetAsync(x => new ValueTuple<int, int, string, string>(x.Id, x.FieldMap.Id, x.FieldMap.DisplayName, x.Value),
                                                   x => !x.IsDeleted && x.IsActive &&
                                                   (!userId.HasValue || x.FieldMap.UserId == userId.Value) && x.FieldMap.IsStandard == false && x.ContactId == contactId,
                                                   null, null, true));
        }
        public async Task<IList<ContactValueMap>> GetAllExistingContactValueMapByContactId(int existingContactId)
        {
            return await (_contactUnitOfWork.ContactValueMapRepository.GetAsync(x => x,
                                                                            x => x.ContactId == existingContactId,
                                                                            null, null, true));
        }
        public async Task<bool> IsContactExist(string email, Guid? userId)
        {
            //TODO: update failed when check isExists
            //var isContactExist = await _contactUnitOfWork.ContactRepository.IsExistsAsync(x => x.Email == email && (!userId.HasValue || x.UserId == userId.Value));
            return false; // isContactExist;
        }

        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            var result = await _contactUnitOfWork.ContactRepository.GetFirstOrDefaultAsync(
                x => x, x => x.Id == contactId,
                null, true);

            if (result == null) throw new NotFoundException(nameof(Contact), contactId);

            return result;
        }

        public async Task<int> GetContactCountAsync(Guid? userId)
        {
            return await _contactUnitOfWork.ContactRepository.GetCountAsync(x => x.UserId == userId);
        }

        public async Task<int> GetContactCountAsync()
        {
            return await _contactUnitOfWork.ContactRepository.GetCountAsync();
        }

        public void Dispose()
        {
            _contactUnitOfWork.Dispose();
        }

    }
}
