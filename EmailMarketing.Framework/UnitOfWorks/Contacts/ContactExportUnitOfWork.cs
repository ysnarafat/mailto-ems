using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
    public class ContactExportUnitOfWork : UnitOfWork, IContactExportUnitOfWork
    {
        public IDownloadQueueRepository DownloadQueueRepository { get; set; }
        public IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }
        public ContactExportUnitOfWork(FrameworkContext dbContext,
            IDownloadQueueRepository downloadQueueRepository,
            IDownloadQueueSubEntityRepository downloadQueueSubEntityRepository) : base(dbContext)
        {
            this.DownloadQueueRepository = downloadQueueRepository;
            this.DownloadQueueSubEntityRepository = downloadQueueSubEntityRepository;
        }
    }
}
