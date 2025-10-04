using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Services.Campaigns;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class CreateEmailTemplateModel : CampaignsBaseModel
    {
        public Guid UserId { get; set; }

        [Display(Name = "Title")]
        public string EmailTemplateName { get; set; }

        [Required]
        [Display(Name = "Email Body")]
        public string EmailTemplateBody { get; set; }

        [Display(Name = "Email Personalize")]
        public bool IsPersonalized { get; set; }

        public CreateEmailTemplateModel(ICampaignService campaignService,
            ICurrentUserService currentUserService,
            IEmailTemplateService emailTemplateService)
            : base(campaignService, currentUserService, emailTemplateService)
        {

        }
        public CreateEmailTemplateModel() : base()
        {

        }

        public async Task CreateEmailTemplate()
        {
            var emailTempalte = new EmailTemplate
            {
                UserId = _currentUserService.UserId,
                EmailTemplateName = this.EmailTemplateName,
                EmailTemplateBody = this.EmailTemplateBody,
                IsPersonalized = this.IsPersonalized
            };

            await _emailTemplateService.AddEmailTemplateAsync(emailTempalte);
        }
    }
}
