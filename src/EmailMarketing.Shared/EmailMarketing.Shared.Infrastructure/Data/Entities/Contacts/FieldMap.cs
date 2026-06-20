using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class FieldMap : IAuditableEntity<int>
{
    public Guid? UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public bool IsStandard { get; set; }
}
