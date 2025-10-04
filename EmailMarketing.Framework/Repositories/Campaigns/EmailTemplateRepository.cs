using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Campaigns;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public class EmailTemplateRepository : Repository<EmailTemplate, int, FrameworkContext>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(FrameworkContext context) : base(context) { }
    }
}
