using EmailMarketing.Common.Exceptions;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public class CampaignReportService : ICampaignReportService
    {
        private readonly ICampaignReportUnitOfWork _campaignReportUnitOfWork;

        public CampaignReportService(ICampaignReportUnitOfWork campaignReportUnitOfWork)
        {
            _campaignReportUnitOfWork = campaignReportUnitOfWork;
        }

        public async Task AddCampaingReportAsync(IList<CampaignReport> campaignReports)
        {
            await _campaignReportUnitOfWork.CampaingReportRepository.AddRangeAsync(campaignReports);
            await _campaignReportUnitOfWork.SaveChangesAsync();
        }

        public async Task EmailOpenTracking(int campaignId, int contactId, string email)
        {
            var campaignReport = await _campaignReportUnitOfWork.CampaingReportRepository.GetFirstOrDefaultAsync(x => x,
                                            x => x.CampaignId == campaignId && x.ContactId == contactId && x.Contact.Email == email,
                                            null, true);

            if (campaignReport == null)
                throw new NotFoundException(nameof(CampaignReport), $"{campaignId}, {contactId}, {email}");

            campaignReport.IsSeen = true;
            campaignReport.SeenDateTime = DateTime.Now;

            await _campaignReportUnitOfWork.CampaingReportRepository.UpdateAsync(campaignReport);
            await _campaignReportUnitOfWork.SaveChangesAsync();
        }
        public async Task<int> GetDeleveredMailCountAsync()
        {
            return await _campaignReportUnitOfWork.CampaingReportRepository.GetCountAsync(x => x.IsDelivered == true);
        }
        public void Dispose()
        {
            _campaignReportUnitOfWork?.Dispose();
        }
    }
}
