using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class Contact : IAuditableEntity<int>
{
    public string Email { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public int? ContactUploadId { get; set; }
    public ContactUpload? ContactUpload { get; set; }
    public IList<ContactValueMap> ContactValueMaps { get; set; } = new List<ContactValueMap>();
    public IList<ContactGroup> ContactGroups { get; set; } = new List<ContactGroup>();
}
