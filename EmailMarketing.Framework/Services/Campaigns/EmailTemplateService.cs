using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private ICampaignUnitOfWork _campaignUnitOfWork;

        public EmailTemplateService(ICampaignUnitOfWork campaignUnitOfWork)
        {
            _campaignUnitOfWork = campaignUnitOfWork;
        }

        public async Task AddEmailTemplateAsync(EmailTemplate emailTemplate)
        {
            var isExists = await _campaignUnitOfWork.EmailTemplateRepository.IsExistsAsync(x => x.UserId == emailTemplate.UserId &&
                                                                                                x.EmailTemplateBody == emailTemplate.EmailTemplateBody);
            if (isExists)
            {
                throw new DuplicationException("Template Already Exists");
            }

            await _campaignUnitOfWork.EmailTemplateRepository.AddAsync(emailTemplate);
            await _campaignUnitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _campaignUnitOfWork?.Dispose();
        }
    }
}
