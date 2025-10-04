using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public class CampaignReportExportUnitOfWork : UnitOfWork, ICampaignReportExportUnitOfWork
    {
        public IDownloadQueueRepository DownloadQueueRepository { get; set; }
        public IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }
        public CampaignReportExportUnitOfWork(FrameworkContext dbContext,
            IDownloadQueueRepository downloadQueueRepository,
            IDownloadQueueSubEntityRepository downloadQueueSubEntityRepository) : base(dbContext)
        {
            this.DownloadQueueRepository = downloadQueueRepository;
            this.DownloadQueueSubEntityRepository = downloadQueueSubEntityRepository;
        }
    }
}
