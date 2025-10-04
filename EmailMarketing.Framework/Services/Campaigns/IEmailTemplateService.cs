using EmailMarketing.Framework.Entities.Campaigns;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public interface IEmailTemplateService : IDisposable
    {
        Task AddEmailTemplateAsync(EmailTemplate emailTemplate);
    }
}
