using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class ContactGroup : IEntity<int>
{
    public int ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
