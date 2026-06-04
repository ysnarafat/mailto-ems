using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Campaigns;
using System;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class CampaignsBaseModel : MemberBaseModel, IDisposable
    {
        protected ICampaignService _campaignService;
        protected IEmailTemplateService _emailTemplateService;
        protected new ICurrentUserService _currentUserService;

        public CampaignsBaseModel(ICampaignService campaignService,
                                ICurrentUserService currentUserService,
                                IEmailTemplateService emailTemplateService)
        {
            _campaignService = campaignService;
            _currentUserService = currentUserService;
            _emailTemplateService = emailTemplateService;
        }

        public CampaignsBaseModel()
        {
            _campaignService = Startup.AutofacContainer.Resolve<ICampaignService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
            _emailTemplateService = Startup.AutofacContainer.Resolve<IEmailTemplateService>();
        }

        public void Dispose()
        {
            _campaignService?.Dispose();
            _emailTemplateService?.Dispose();
        }
    }
}
