using EmailMarketing.Data;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class ContactUploadFieldMap : IEntity<int>
    {
        public int Index { get; set; }
        public int FieldMapId { get; set; }
        public FieldMap FieldMap { get; set; }
        public int ContactUploadId { get; set; }
        public ContactUpload ContactUpload { get; set; }
    }
}
