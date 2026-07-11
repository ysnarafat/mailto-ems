using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public interface IContactExportUnitOfWork : IUnitOfWork
{
    IDownloadQueueRepository DownloadQueueRepository { get; set; }
    IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }
}

