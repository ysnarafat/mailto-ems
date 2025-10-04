using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
    public interface IContactExportUnitOfWork : IUnitOfWork
    {
        IDownloadQueueRepository DownloadQueueRepository { get; set; }
        IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }
    }
}
