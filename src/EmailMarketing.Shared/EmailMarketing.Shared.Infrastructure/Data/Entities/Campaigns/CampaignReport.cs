using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Campaigns;

public class CampaignReport : IAuditableEntity<int>
{
    public int CampaignId { get; set; }
    public Campaign Campaign { get; set; } = null!;
    public int ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    public Guid SMTPConfigId { get; set; }
    public SMTPConfig SMTPConfig { get; set; } = null!;
    public bool IsDelivered { get; set; }
    public bool IsSeen { get; set; }
    public bool IsPersonalized { get; set; }
    public DateTime SendDateTime { get; set; }
    public DateTime? SeenDateTime { get; set; }
}
