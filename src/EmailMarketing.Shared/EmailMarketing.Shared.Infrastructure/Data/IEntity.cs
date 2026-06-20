namespace EmailMarketing.Shared.Infrastructure.Data;

public abstract class IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }

    protected IEntity()
    {
        IsDeleted = false;
        IsActive = true;
    }
}
