using EmailMarketing.Data;
using EmailMarketing.Framework.Context;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public class CampaignRepository : Repository<Entities.Campaigns.Campaign, int, FrameworkContext>, ICampaignRepository
    {
        public CampaignRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
