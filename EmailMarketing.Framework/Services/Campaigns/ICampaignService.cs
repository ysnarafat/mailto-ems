using EmailMarketing.Framework.Entities.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EmailMarketing.Framework.Services.Campaigns
{
    public interface ICampaignService : IDisposable
    {
        Task<(IList<Campaign> Items, int Total, int TotalFilter)> GetAllCampaignAsync(
           Guid? userId,
           string searchText,
           string orderBy,
           int pageIndex,
           int pageSize);
        Task<(IList<CampaignReport> Items, int Total, int TotalFilter)> GetAllCampaignReportAsync(
        Guid? userId,
        int campaignId,
        string searchText,
        string orderBy,
        int pageIndex,
        int pageSize);
        Task<IList<(int Value, string Text, int Count)>> GetAllGroupsAsync(Guid? userId);
        Task<IList<EmailTemplate>> GetEmailTemplateByUserIdAsync(Guid? userId);
        Task AddCampaign(Campaign campaign);
        Task<IList<Campaign>> GetAllProcessingCampaign();
        Task<Campaign> GetAllEmailByCampaignId(int campaignId);
        Task UpdateCampaignAsync(Campaign campaign);
        Task<Campaign> GetCampaignByIdAsync(Guid? userId, int campaignId);
        Task<int> GetCampaignCountAsync(Guid? userId);
        Task<int> GetCampaignCountAsync();
        Task<Campaign> ActivateCampaignAsync(int id);
    }
}
