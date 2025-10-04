using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public interface IContactExportService : IDisposable
    {
        Task<(IList<DownloadQueue> Items, int Total, int TotalFilter)> GetAllContactExportFileFromDownloadQueueAsync(
          Guid? userId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize);
        Task<IList<(int Value, string Text, int Count)>> GetAllGroupAsync(Guid? userId);
        Task<IList<Contact>> GetAllContactAsync(Guid? userId);
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<IList<ContactGroup>> GetAllContactGroupByUserIdAsync(Guid? userId, int groupId);
        Task SaveDownloadQueueAsync(DownloadQueue downloadQueue);
        Task UpdateDownloadQueueAsync(DownloadQueue downloadQueue);
        Task<IList<DownloadQueue>> GetDownloadQueueAsync();
        Task<DownloadQueue> GetDownloadQueueByIdAsync(int downloadQueueId);
        Task AddDownloadQueueSubEntitiesAsync(IList<DownloadQueueSubEntity> downloadQueueSubEntities);
        Task ExcelExportForAllContactsAsync(DownloadQueue downloadQueue);
        Task ExcelExportForGroupwiseContactsAsync(DownloadQueue downloadQueue);
    }
}