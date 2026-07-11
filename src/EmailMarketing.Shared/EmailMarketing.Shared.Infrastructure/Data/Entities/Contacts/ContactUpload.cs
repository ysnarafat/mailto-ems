using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class ContactUpload : IAuditableEntity<int>
{
    public Guid UserId { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool IsSucceed { get; set; }
    public bool IsUpdateExisting { get; set; }
    public bool HasColumnHeader { get; set; }
    public bool IsSendEmailNotify { get; set; }
    public string SendEmailAddress { get; set; } = string.Empty;
    public int SucceedEntryCount { get; set; }
    public bool IsProcessing { get; set; }
    public IList<ContactUploadFieldMap> ContactUploadFieldMaps { get; set; } = new List<ContactUploadFieldMap>();
    public IList<ContactUploadGroup> ContactUploadGroups { get; set; } = new List<ContactUploadGroup>();
}
