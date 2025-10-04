using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Campaigns;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public interface ICampaignUnitOfWork : IUnitOfWork
    {
        ICampaignReportRepository CampaignReportRepository { get; set; }
        public ICampaignRepository CampaignRepository { get; set; }
        public IEmailTemplateRepository EmailTemplateRepository { get; set; }
    }
}
