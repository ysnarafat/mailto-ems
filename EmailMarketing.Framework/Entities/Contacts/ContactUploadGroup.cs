using EmailMarketing.Data;
using EmailMarketing.Framework.Entities.Groups;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class ContactUploadGroup : IEntity<int>
    {
        public int ContactUploadId { get; set; }
        public ContactUpload ContactUpload { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
