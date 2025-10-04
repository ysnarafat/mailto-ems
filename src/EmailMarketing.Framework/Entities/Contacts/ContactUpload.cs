using EmailMarketing.Data;
using System;
using System.Collections.Generic;

namespace EmailMarketing.Framework.Entities.Contacts
{
    public class ContactUpload : IAuditableEntity<int>
    {
        public Guid UserId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public bool IsSucceed { get; set; }
        public bool IsUpdateExisting { get; set; }
        public bool HasColumnHeader { get; set; }
        public bool IsSendEmailNotify { get; set; }
        public string SendEmailAddress { get; set; }
        public int SucceedEntryCount { get; set; }
        public bool IsProcessing { get; set; }

        public IList<ContactUploadFieldMap> ContactUploadFieldMaps { get; set; }
        public IList<ContactUploadGroup> ContactUploadGroups { get; set; }

        public ContactUpload()
        {
            this.ContactUploadFieldMaps = new List<ContactUploadFieldMap>();
            this.ContactUploadGroups = new List<ContactUploadGroup>();
        }
    }
}
