using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing.Enums;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;

public class DownloadQueue : IAuditableEntity<int>
{
    public DownloadQueueFor DownloadQueueFor { get; set; }
    public Guid UserId { get; set; }
    public bool IsSendEmailNotify { get; set; }
    public string SendEmailAddress { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool IsSucceed { get; set; }
    public int SucceedEntryCount { get; set; }
    public bool IsProcessing { get; set; }
    public IList<DownloadQueueSubEntity> DownloadQueueSubEntities { get; set; } = new List<DownloadQueueSubEntity>();
}
