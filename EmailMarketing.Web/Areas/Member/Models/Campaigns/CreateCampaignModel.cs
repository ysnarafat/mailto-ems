using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Entities.SMTP;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.Services.SMTP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Campaigns
{
    public class CreateCampaignModel : CampaignsBaseModel
    {
        public Guid UserId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Email Subject")]
        public string EmailSubject { get; set; }
        [Display(Name = "Send DateTime")]
        public DateTime? SendDateTime { get; set; }
        [Display(Name = "Notify Me")]
        public bool IsSendEmailNotify { get; set; }
        [Display(Name = "Sender Email")]
        public string SendEmailAddress { get; set; }
        public bool SendNow { get; set; }
        [Display(Name = "Draft")]
        public bool IsDraft { get; set; }
        [Display(Name = "Personalize")]
        public bool isPersonalized { get; set; }
        public int? SelectedTemplateId { get; set; }
        [Display(Name = "Template Title")]
        public string EmailTemplateTitle { get; set; }
        [Display(Name = "Email Body")]
        public string EmailTemplateBody { get; set; }
        [Required]
        [Display(Name = "Select SMTP")]
        public Guid SMTPConfigId { get; set; }
        public IList<EmailTemplate> EmailTemplateList { get; set; }
        public IList<CampaignValueTextModel> GroupSelectList { get; set; }
        public IList<CampaignGroup> CampaignGroups { get; set; }
        public IList<SMTPConfig> SMTPConfigList { get; set; }

        private IDateTime _dateTime;
        private ISMTPService _sMTPService;

        public CreateCampaignModel(ICampaignService campaignService,
            ICurrentUserService currentUserService,
            IEmailTemplateService emailTemplateService,
            ISMTPService sMTPService,
            IDateTime dateTime)
            : base(campaignService, currentUserService, emailTemplateService)
        {
            _dateTime = dateTime;
            _sMTPService = sMTPService;
        }
        public CreateCampaignModel() : base()
        {
            _dateTime = Startup.AutofacContainer.Resolve<IDateTime>();
            _sMTPService = Startup.AutofacContainer.Resolve<ISMTPService>();
        }

        public async Task<IList<CampaignValueTextModel>> GetAllGroupForSelectAsync()
        {
            return (await _campaignService.GetAllGroupsAsync(_currentUserService.UserId))
                                           .Select(x => new CampaignValueTextModel { Value = x.Value, Text = x.Text, Count = x.Count, IsChecked = false }).ToList();
        }

        public async Task<IList<EmailTemplate>> GetTemplateByUserIDAsync()
        {
            return (await _campaignService.GetEmailTemplateByUserIdAsync(_currentUserService.UserId));
        }

        public async Task<IList<SMTPConfig>> GetAllSMTPConfigByUserIdAsync()
        {
            return await _sMTPService.GetAllSMTPConfig(_currentUserService.UserId);
        }

        public async Task SaveCampaignAsync()
        {

            if (!this.GroupSelectList.Any(x => x.IsChecked))
            {
                throw new Exception("Please select at least one group.");
            }

            var campaign = new Campaign
            {
                UserId = _currentUserService.UserId,
                Name = this.Name,
                Description = this.Description,
                EmailSubject = this.EmailSubject,
                SendDateTime = this.SendNow ? _dateTime.Now : this.SendDateTime ?? _dateTime.Now,
                IsDraft = this.IsDraft,
                IsSendEmailNotify = this.IsSendEmailNotify,
                SendEmailAddress = this.SendEmailAddress,
                SMTPConfigId = this.SMTPConfigId,
                IsPersonalized = this.isPersonalized,
                IsProcessing = this.IsDraft ? false : true,
            };


            foreach (var item in GroupSelectList)
            {
                if (item.IsChecked)
                {
                    var campaignGroup = new CampaignGroup
                    {
                        GroupId = item.Value
                    };

                    campaign.CampaignGroups.Add(campaignGroup);
                }

            }

            var hasTemplateAssign = false;
            if (this.SelectedTemplateId.HasValue && this.SelectedTemplateId.Value != 0)
            {
                campaign.EmailTemplateId = this.SelectedTemplateId.Value;
                campaign.IsPersonalized = EmailTemplateList.FirstOrDefault(x => x.Id == this.SelectedTemplateId.Value).IsPersonalized;
                hasTemplateAssign = true;
            }
            else if (!string.IsNullOrWhiteSpace(this.EmailTemplateBody))
            {
                var emailTemplate = new EmailTemplate
                {
                    EmailTemplateName = this.EmailTemplateTitle,
                    EmailTemplateBody = this.EmailTemplateBody,
                    IsPersonalized = this.isPersonalized,
                    UserId = _currentUserService.UserId
                };
                campaign.EmailTemplate = emailTemplate;
                hasTemplateAssign = true;
            }
            if (hasTemplateAssign)
            {
                await _campaignService.AddCampaign(campaign);
            }
            else
            {
                throw new Exception("You must select one Template or Create a new Template.");
            }



        }
    }
}
