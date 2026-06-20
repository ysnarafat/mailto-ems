using Microsoft.EntityFrameworkCore.Storage;

namespace EmailMarketing.Shared.Infrastructure.Data;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransaction();
}
