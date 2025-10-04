using EmailMarketing.Data;
using EmailMarketing.Framework.Entities.SMTP;
using System;
using System.Collections.Generic;

namespace EmailMarketing.Framework.Entities.Campaigns
{
    public class Campaign : IAuditableEntity<int>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailSubject { get; set; }
        public int EmailTemplateId { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public DateTime SendDateTime { get; set; }
        public bool IsSendEmailNotify { get; set; }
        public string SendEmailAddress { get; set; }
        public bool IsDraft { get; set; }
        public bool IsProcessing { get; set; }
        public bool IsSucceed { get; set; }
        public Guid SMTPConfigId { get; set; }
        public SMTPConfig SMTPConfig { get; set; }
        public bool IsPersonalized { get; set; }

        public IList<CampaignGroup> CampaignGroups { get; set; }
        public IList<CampaignReport> CampaignReports { get; set; }

        public Campaign()
        {
            this.CampaignGroups = new List<CampaignGroup>();
            this.CampaignReports = new List<CampaignReport>();
        }
    }
}
