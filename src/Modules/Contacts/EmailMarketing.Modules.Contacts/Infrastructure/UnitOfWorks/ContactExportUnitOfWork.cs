using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public class ContactExportUnitOfWork : UnitOfWork, IContactExportUnitOfWork
{
    public IDownloadQueueRepository DownloadQueueRepository { get; set; }
    public IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }

    public ContactExportUnitOfWork(
        ApplicationDbContext dbContext,
        IDownloadQueueRepository downloadQueueRepository,
        IDownloadQueueSubEntityRepository downloadQueueSubEntityRepository)
        : base(dbContext)
    {
        DownloadQueueRepository = downloadQueueRepository;
        DownloadQueueSubEntityRepository = downloadQueueSubEntityRepository;
    }
}

