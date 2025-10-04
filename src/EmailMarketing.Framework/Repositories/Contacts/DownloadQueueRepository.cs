using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class DownloadQueueRepository : Repository<DownloadQueue, int, FrameworkContext>, IDownloadQueueRepository
    {
        public DownloadQueueRepository(FrameworkContext dbContext)
           : base(dbContext)
        {

        }
    }
}
