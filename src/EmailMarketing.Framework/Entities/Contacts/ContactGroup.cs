using EmailMarketing.Data;
using EmailMarketing.Framework.Entities.Groups;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class ContactGroup : IEntity<int>
    {
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
