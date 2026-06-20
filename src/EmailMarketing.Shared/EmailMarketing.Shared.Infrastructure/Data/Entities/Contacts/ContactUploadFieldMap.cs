using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class ContactUploadFieldMap : IEntity<int>
{
    public int Index { get; set; }
    public int FieldMapId { get; set; }
    public FieldMap FieldMap { get; set; } = null!;
    public int ContactUploadId { get; set; }
    public ContactUpload ContactUpload { get; set; } = null!;
}
