using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Campaigns
{
    public interface ICampaignReportExportUnitOfWork : IUnitOfWork
    {
        IDownloadQueueRepository DownloadQueueRepository { get; set; }
        IDownloadQueueSubEntityRepository DownloadQueueSubEntityRepository { get; set; }
    }
}
