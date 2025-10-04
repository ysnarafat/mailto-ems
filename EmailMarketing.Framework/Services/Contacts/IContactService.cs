
using EmailMarketing.Framework.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public interface IContactService : IDisposable
    {
        Task<IList<ContactValueMap>> GetAllExistingContactValueMapByContactId(int existingContactId);
        Task<(IList<Entities.Contacts.Contact> Items, int Total, int TotalFilter)> GetAllContactAsync(
            Guid? userId,
            string searchText,
            string orderBy,
            int pageIndex,
            int pageSize);
        Task<(IList<Contact> Items, int Total, int TotalFilter)> GetContactByContactUploadIdAsync(Guid? userId, int contactUploadId,
           string searchText,
           string orderBy,
           int pageIndex,
           int pageSize);
        Task<ContactValueMap> GetContactValueMapByIdAsync(int id);
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<Entities.Contacts.Contact> GetByIdAsync(int id);
        Task<Entities.Contacts.Contact> DeleteAsync(int id);
        Task DeleteContactGroupAsync(int contactId);
        Task<int> GroupContactCountAsync(int id);
        Task<Entities.Groups.Group> GetGroupByIdAsync(int id);
        Task<IList<(int Value, string Text, int Count)>> GetAllGroupsAsync(Guid? userId);
        Task<IList<(int Value, string Text)>> GetAllContactValueMapsStandard();
        Task<IList<(int Id, int Value, string Text, string Input)>> GetAllSelectedContactValueMapsStandard(int contactId);
        Task<IList<(int Value, string Text)>> GetAllContactValueMapsCustom(Guid? userId);
        Task<IList<(int Id, int Value, string Text, string Input)>> GetAllContactValueMapsCustom(Guid? userId, int contactId);
        Task AddContact(Contact contact);
        Task AddContacValueMaps(IList<ContactValueMap> contactValueMap);
        Task AddContactGroups(IList<ContactGroup> contactGroups);
        Task<bool> IsContactExist(string email, Guid? userId);
        Task UpdateAsync(Contact contact);
        Task DeleteRangeAsync(IList<ContactValueMap> contact);
        Task<int> GetContactCountAsync(Guid? userId);
        Task<int> GetContactCountAsync();
    }
}
