using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Exceptions;
using EmailMarketing.Shared.Infrastructure.Extensions;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Modules.Contacts.UnitOfWorks;

namespace EmailMarketing.Modules.Contacts.Services;

public class FieldMapService : IFieldMapService
{
    private IFieldMapUnitOfWork _fieldMapUnitOfWork;

    public FieldMapService(IFieldMapUnitOfWork fieldMapUnitOfWork)
    {
        _fieldMapUnitOfWork = fieldMapUnitOfWork;
    }

    public async Task AddAsync(FieldMap entity)
    {
        var isExists = await _fieldMapUnitOfWork.FieldMapRepository.IsExistsAsync(
            x => x.DisplayName == entity.DisplayName && (!x.UserId.HasValue || x.UserId == entity.UserId.Value));
        if (isExists)
            throw new DuplicationException(nameof(entity.DisplayName));

        await _fieldMapUnitOfWork.FieldMapRepository.AddAsync(entity);
        await _fieldMapUnitOfWork.SaveChangesAsync();
    }

    public async Task<FieldMap> ActivateUpdateAsync(int id)
    {
        var fieldMap = await GetByIdAsync(id);
        if (fieldMap == null) throw new NotFoundException(nameof(FieldMap), id);

        fieldMap.IsActive = !fieldMap.IsActive;

        await _fieldMapUnitOfWork.FieldMapRepository.UpdateAsync(fieldMap);
        await _fieldMapUnitOfWork.SaveChangesAsync();
        return fieldMap;
    }

    public async Task<(IList<FieldMap> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
    {
        var columnsMap = new Dictionary<string, Expression<Func<FieldMap, object>>>()
        {
            ["DisplayName"] = v => v.DisplayName
        };

        var result = await _fieldMapUnitOfWork.FieldMapRepository.GetAsync<FieldMap>(
            x => x, x => !x.IsStandard && (!userId.HasValue || x.UserId == userId.Value) &&
                         (string.IsNullOrWhiteSpace(searchText) || x.DisplayName.Contains(searchText)),
            x => x.ApplyOrdering(columnsMap, orderBy), null,
            pageIndex, pageSize, true);

        result.Total = await _fieldMapUnitOfWork.FieldMapRepository.GetCountAsync(x => x.UserId == userId);

        return (result.Items, result.Total, result.TotalFilter);
    }

    public async Task<FieldMap> GetByIdAsync(int id)
    {
        var result = await _fieldMapUnitOfWork.FieldMapRepository.GetByIdAsync(id);
        if (result == null) throw new NotFoundException(nameof(FieldMap), id);
        return result;
    }

    public async Task UpdateAsync(FieldMap entity)
    {
        var isExists = await _fieldMapUnitOfWork.FieldMapRepository.IsExistsAsync(
            x => x.DisplayName == entity.DisplayName && x.Id != entity.Id && (!x.UserId.HasValue || x.UserId == entity.UserId));
        if (isExists)
            throw new DuplicationException(nameof(entity.DisplayName));

        var updateEntity = await GetByIdAsync(entity.Id);
        updateEntity.DisplayName = entity.DisplayName;

        await _fieldMapUnitOfWork.FieldMapRepository.UpdateAsync(updateEntity);
        await _fieldMapUnitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        _fieldMapUnitOfWork?.Dispose();
    }
}


