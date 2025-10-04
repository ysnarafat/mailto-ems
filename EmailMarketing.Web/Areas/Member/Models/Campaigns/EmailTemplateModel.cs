using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Services.Campaigns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class EmailTemplateModel : CampaignBaseModel
    {
        public IList<EmailTemplate> EmailTemplateList { get; set; }
        public EmailTemplateModel(ICampaignService campaignService,
            ICurrentUserService currentUserService)
            : base(campaignService, currentUserService)
        {

        }
        public EmailTemplateModel() : base()
        {

        }
        public async Task<IList<EmailTemplate>> GetTemplateByUserIDAsync()
        {
            return (await _campaignService.GetEmailTemplateByUserIdAsync(_currentUserService.UserId));
        }
    }
}
