using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class ContactUploadGroup : IEntity<int>
{
    public int ContactUploadId { get; set; }
    public ContactUpload ContactUpload { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
