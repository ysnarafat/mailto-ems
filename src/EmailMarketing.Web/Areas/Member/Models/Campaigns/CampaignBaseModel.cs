using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Campaigns;
using System;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{

    public class CampaignBaseModel : MemberBaseModel, IDisposable
    {
        protected readonly ICampaignService _campaignService;
        protected readonly ICampaignReportExportService _campaignReportExportService;
        protected readonly ICurrentUserService _currentUserService;
        public CampaignBaseModel(ICampaignService campaignService, ICampaignReportExportService campaignREService,

            ICurrentUserService currentUserService)
        {
            _campaignService = campaignService;
            _campaignReportExportService = campaignREService;
            _currentUserService = currentUserService;
        }

        public CampaignBaseModel(ICampaignService campaignService,
            ICurrentUserService currentUserService)
        {
            _campaignService = campaignService;
            _currentUserService = currentUserService;
        }

        public CampaignBaseModel()
        {
            _campaignService = Startup.AutofacContainer.Resolve<ICampaignService>();
            _campaignReportExportService = Startup.AutofacContainer.Resolve<ICampaignReportExportService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
        }
        public void Dispose()
        {
            _campaignService?.Dispose();
        }
    }
}
