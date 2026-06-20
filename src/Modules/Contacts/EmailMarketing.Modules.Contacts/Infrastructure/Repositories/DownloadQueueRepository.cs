using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class DownloadQueueRepository : Repository<DownloadQueue, int, ApplicationDbContext>, IDownloadQueueRepository
{
    public DownloadQueueRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


