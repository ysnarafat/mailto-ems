using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class DownloadQueueSubEntityRepository : Repository<DownloadQueueSubEntity, int, ApplicationDbContext>, IDownloadQueueSubEntityRepository
{
    public DownloadQueueSubEntityRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


