using System.Linq.Expressions;

namespace EmailMarketing.Shared.Infrastructure.Extensions;

public static class IQueryableExtension
{
    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(
        this IQueryable<TEntity> query,
        Dictionary<string, Expression<Func<TEntity, object>>> columnsMap,
        string multipleSortBy)
    {
        IOrderedQueryable<TEntity> orderedQuery = (IOrderedQueryable<TEntity>)query;
        var orderByList = multipleSortBy.Split(',');
        if (!orderByList.Any()) return orderedQuery;

        var orderBy = orderByList[0];
        var prop = orderBy.Split(' ')[0];

        if (orderBy.Split(' ')[1] == "asc" && columnsMap.ContainsKey(prop))
            orderedQuery = orderedQuery.OrderBy(columnsMap[prop]);
        else if (columnsMap.ContainsKey(prop))
            orderedQuery = orderedQuery.OrderByDescending(columnsMap[prop]);
        else
            orderedQuery = orderedQuery.OrderBy(x => x);

        for (int i = 1; i < orderByList.Length; i++)
        {
            orderBy = orderByList[i];
            prop = orderBy.Split(' ')[0];
            if (orderBy.Split(' ')[1] == "asc" && columnsMap.ContainsKey(prop))
                orderedQuery = orderedQuery.ThenBy(columnsMap[prop]);
            else if (columnsMap.ContainsKey(prop))
                orderedQuery = orderedQuery.ThenByDescending(columnsMap[prop]);
        }
        return orderedQuery;
    }

    public static IOrderedQueryable<TEntity> ApplyOrdering<TEntity>(
        this IQueryable<TEntity> query,
        Dictionary<string, Expression<Func<TEntity, object>>> columnsMap,
        string sortBy, bool isAsc = true)
    {
        IOrderedQueryable<TEntity> orderedQuery = (IOrderedQueryable<TEntity>)query;
        if (string.IsNullOrEmpty(sortBy) || columnsMap == null || !columnsMap.ContainsKey(sortBy))
            return orderedQuery;
        return isAsc ? orderedQuery.OrderBy(columnsMap[sortBy]) : orderedQuery.OrderByDescending(columnsMap[sortBy]);
    }
}
