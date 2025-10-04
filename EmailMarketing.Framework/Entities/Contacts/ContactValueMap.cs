using EmailMarketing.Data;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class ContactValueMap : IEntity<int>
    {
        //public string Name { get; set; }
        public string Value { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
        public int FieldMapId { get; set; }
        public FieldMap FieldMap { get; set; }
    }
}
