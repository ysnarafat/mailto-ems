using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public interface IDownloadQueueSubEntityRepository : IRepository<DownloadQueueSubEntity, int, ApplicationDbContext>
{
}


