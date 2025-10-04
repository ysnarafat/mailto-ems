using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public class CampaignReportExportRepository : Repository<DownloadQueue, int, FrameworkContext>, ICampaignReportExportRepository
    {
        public CampaignReportExportRepository(FrameworkContext dbContext)
           : base(dbContext)
        {

        }
    }
}
