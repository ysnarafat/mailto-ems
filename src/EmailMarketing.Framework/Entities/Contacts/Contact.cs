using EmailMarketing.Data;
using System;
using System.Collections.Generic;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class Contact : IAuditableEntity<int>
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public int? ContactUploadId { get; set; }
        public ContactUpload ContactUpload { get; set; }

        public IList<ContactValueMap> ContactValueMaps { get; set; }
        public IList<ContactGroup> ContactGroups { get; set; }

        public Contact()
        {
            this.ContactValueMaps = new List<ContactValueMap>();
            this.ContactGroups = new List<ContactGroup>();
        }
    }
}
