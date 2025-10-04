using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public interface IDownloadQueueRepository : IRepository<DownloadQueue, int, FrameworkContext>
    {

    }
}
