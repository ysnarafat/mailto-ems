using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Campaigns
{
    public interface ICampaignReportExportService : IDisposable
    {
        Task<DownloadQueueSubEntity> GetAllDownloadQueueSubEntityById(int id);
        Task<(IList<DownloadQueue> Items, int Total, int TotalFilter)> GetAllCampaignReportsFromDownloadQueueAsync(
          Guid? userId,
          string searchText,
          string orderBy,
          int pageIndex,
          int pageSize);
        Task<IList<CampaignReport>> GetAllCampaignReportAsync(
            Guid? userId);
        Task<IList<object>> GetCampaignsForSelectAsync(Guid? userId);
        Task SaveDownloadQueueAsync(DownloadQueue downloadQueue);
        Task<IList<DownloadQueue>> GetDownloadQueue();
        Task<DownloadQueue> GetDownloadQueueByIdAsync(int campaingReportId);
        Task UpdateDownloadQueueAync(DownloadQueue downloadQueue);
        Task AddDownloadQueueSubEntities(DownloadQueueSubEntity downloadQueueSubEntity);
        Task ExcelExportForAllCampaignAsync(DownloadQueue downloadQueue);
        Task ExcelExportForCampaignWiseAsync(DownloadQueue downloadQueue);
        Task<IList<CampaignReport>> GetCampaignWiseReportAsync(Guid? userId, int campaignId);
    }
}
