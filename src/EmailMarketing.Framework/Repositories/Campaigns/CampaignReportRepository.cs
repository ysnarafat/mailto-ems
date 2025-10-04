using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Campaigns;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public class CampaignReportRepository : Repository<CampaignReport, int, FrameworkContext>, ICampaignReportRepository
    {
        public CampaignReportRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
