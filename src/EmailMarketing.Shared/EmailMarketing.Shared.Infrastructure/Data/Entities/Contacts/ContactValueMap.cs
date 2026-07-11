using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

public class ContactValueMap : IEntity<int>
{
    public string Value { get; set; } = string.Empty;
    public int ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    public int FieldMapId { get; set; }
    public FieldMap FieldMap { get; set; } = null!;
}
