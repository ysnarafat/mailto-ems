using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class DownloadQueueSubEntityRepository : Repository<DownloadQueueSubEntity, int, FrameworkContext>, IDownloadQueueSubEntityRepository
    {
        public DownloadQueueSubEntityRepository(FrameworkContext dbContext)
           : base(dbContext)
        {

        }
    }
}
