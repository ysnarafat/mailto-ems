using EmailMarketing.Data;

namespace EmailMarketing.Framework.Entities
{
    public class DownloadQueueSubEntity : IEntity<int>
    {
        public int DownloadQueueId { get; set; }
        public DownloadQueue DownloadQueue { get; set; }
        public int DownloadQueueSubEntityId { get; set; }
    }
}
