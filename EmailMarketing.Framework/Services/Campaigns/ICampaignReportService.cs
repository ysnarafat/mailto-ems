using EmailMarketing.Framework.Entities.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public interface ICampaignReportService : IDisposable
    {
        Task AddCampaingReportAsync(IList<CampaignReport> campaignReports);
        Task EmailOpenTracking(int campaignId, int contactId, string email);
        Task<int> GetDeleveredMailCountAsync();
    }
}
