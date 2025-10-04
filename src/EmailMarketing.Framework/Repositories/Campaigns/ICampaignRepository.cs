using EmailMarketing.Data;
using EmailMarketing.Framework.Context;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public interface ICampaignRepository : IRepository<Entities.Campaigns.Campaign, int, FrameworkContext>
    {

    }
}
