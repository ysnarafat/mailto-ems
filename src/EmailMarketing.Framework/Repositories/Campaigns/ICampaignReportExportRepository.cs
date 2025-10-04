using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public interface ICampaignReportExportRepository : IRepository<DownloadQueue, int, FrameworkContext>
    {

    }
}
