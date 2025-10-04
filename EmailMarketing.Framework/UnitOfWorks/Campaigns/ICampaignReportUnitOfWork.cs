using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Campaigns;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public interface ICampaignReportUnitOfWork : IUnitOfWork
    {
        public ICampaignReportRepository CampaingReportRepository { get; set; }
    }
}
