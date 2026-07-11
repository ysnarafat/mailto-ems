using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;

public class DownloadQueueSubEntity : IEntity<int>
{
    public int DownloadQueueId { get; set; }
    public DownloadQueue DownloadQueue { get; set; } = null!;
    public int DownloadQueueSubEntityId { get; set; }
}
