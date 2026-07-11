using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Campaigns;

public class Campaign : IAuditableEntity<int>
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EmailSubject { get; set; } = string.Empty;
    public int EmailTemplateId { get; set; }
    public EmailTemplate EmailTemplate { get; set; } = null!;
    public DateTime SendDateTime { get; set; }
    public bool IsSendEmailNotify { get; set; }
    public string SendEmailAddress { get; set; } = string.Empty;
    public bool IsDraft { get; set; }
    public bool IsProcessing { get; set; }
    public bool IsSucceed { get; set; }
    public Guid SMTPConfigId { get; set; }
    public SMTPConfig SMTPConfig { get; set; } = null!;
    public bool IsPersonalized { get; set; }

    public IList<CampaignGroup> CampaignGroups { get; set; } = new List<CampaignGroup>();
    public IList<CampaignReport> CampaignReports { get; set; } = new List<CampaignReport>();
}
