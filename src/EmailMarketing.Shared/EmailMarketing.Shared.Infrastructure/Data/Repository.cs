using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EmailMarketing.Shared.Infrastructure.Data;

public abstract class Repository<TEntity, TKey, TContext> : IRepository<TEntity, TKey, TContext>
    where TEntity : IEntity<TKey>
    where TContext : DbContext
{
    protected TContext _dbContext;
    protected DbSet<TEntity> _dbSet;

    protected Repository(TContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task<IList<TResult>> GetAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null) query = orderBy(query);
        if (disableTracking) query = query.AsNoTracking();
        return await query.Select(selector).ToListAsync();
    }

    public virtual async Task<(IList<TResult> Items, int Total, int TotalFilter)> GetAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1, int pageSize = 10,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();
        int total = await query.CountAsync();
        int totalFilter = total;
        if (include != null) query = include(query);
        if (predicate != null) { query = query.Where(predicate); totalFilter = await query.CountAsync(); }
        if (orderBy != null) query = orderBy(query);
        if (disableTracking) query = query.AsNoTracking();
        var result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selector).ToListAsync();
        return (result, total, totalFilter);
    }

    public virtual async Task<TResult?> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();
        if (include != null) query = include(query);
        if (predicate != null) query = query.Where(predicate);
        if (disableTracking) query = query.AsNoTracking();
        return await query.Select(selector).FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);

    public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = _dbSet.AsQueryable();
        if (predicate != null) query = query.Where(predicate);
        return await query.CountAsync();
    }

    public virtual async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate)
        => await _dbSet.AsQueryable().AnyAsync(predicate);

    public virtual async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
    public virtual async Task AddRangeAsync(IList<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

    public virtual Task UpdateAsync(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IList<TEntity> entities)
    {
        _dbContext.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null) await DeleteAsync(entity);
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _dbSet.AsQueryable().Where(predicate);
        if (query.Any()) _dbSet.RemoveRange(query);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IList<TEntity> entities)
    {
        _dbContext.RemoveRange(entities);
        return Task.CompletedTask;
    }
}
