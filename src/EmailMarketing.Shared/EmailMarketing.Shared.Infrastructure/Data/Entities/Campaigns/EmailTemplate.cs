using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Campaigns;

public class EmailTemplate : IAuditableEntity<int>
{
    public Guid UserId { get; set; }
    public string EmailTemplateName { get; set; } = string.Empty;
    public string EmailTemplateBody { get; set; } = string.Empty;
    public bool IsPersonalized { get; set; }
}
