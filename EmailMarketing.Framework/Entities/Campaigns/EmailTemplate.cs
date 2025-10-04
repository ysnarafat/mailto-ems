using EmailMarketing.Data;
using System;

namespace EmailMarketing.Framework.Entities.Campaigns
{
    public class EmailTemplate : IAuditableEntity<int>
    {
        public Guid UserId { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateBody { get; set; }
        public bool IsPersonalized { get; set; }
    }
}
