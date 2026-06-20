using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmailMarketing.Shared.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    protected readonly DbContext _dbContext;

    public UnitOfWork(DbContext dbContext) => _dbContext = dbContext;

    public void SaveChanges() => _dbContext?.SaveChanges();
    public Task SaveChangesAsync() => _dbContext?.SaveChangesAsync() ?? Task.CompletedTask;

    public async Task<IDbContextTransaction> BeginTransaction()
        => await _dbContext.Database.BeginTransactionAsync();

    ~UnitOfWork() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
            _dbContext?.Dispose();
        _disposed = true;
    }
}
