using EmailMarketing.Data;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.SMTP;
using System;

namespace EmailMarketing.Framework.Entities.Campaigns
{
    public class CampaignReport : IAuditableEntity<int>
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid SMTPConfigId { get; set; }
        public SMTPConfig SMTPConfig { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsSeen { get; set; }
        public bool IsPersonalized { get; set; }
        public DateTime SendDateTime { get; set; }
        public DateTime? SeenDateTime { get; set; }
    }
}
