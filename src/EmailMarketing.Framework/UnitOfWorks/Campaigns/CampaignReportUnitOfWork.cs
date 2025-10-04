using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Campaigns;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public class CampaignReportUnitOfWork : UnitOfWork, ICampaignReportUnitOfWork
    {
        public ICampaignReportRepository CampaingReportRepository { get; set; }
        public CampaignReportUnitOfWork(FrameworkContext dbContext,
            ICampaignReportRepository campaingReportRepository) : base(dbContext)
        {
            this.CampaingReportRepository = campaingReportRepository;
        }
    }
}