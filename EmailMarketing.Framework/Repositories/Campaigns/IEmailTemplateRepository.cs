using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Campaigns;

namespace EmailMarketing.Framework.Repositories.Campaigns
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplate, int, FrameworkContext>
    {
    }
}
