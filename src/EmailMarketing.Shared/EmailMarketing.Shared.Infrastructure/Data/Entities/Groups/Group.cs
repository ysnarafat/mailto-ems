using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;

public class Group : IAuditableEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public IList<ContactGroup> ContactGroups { get; set; } = new List<ContactGroup>();
    public IList<ContactUploadGroup> ContactUploadGroups { get; set; } = new List<ContactUploadGroup>();
}
