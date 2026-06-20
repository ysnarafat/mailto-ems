using EmailMarketing.Shared.Infrastructure.Exceptions;
using EmailMarketing.Shared.Infrastructure.Extensions;
using EmailMarketing.Shared.Abstractions.Services;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;
using EmailMarketing.Modules.Groups.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailMarketing.Modules.Groups.Services;

public class GroupService : IGroupService
{
    private readonly IGroupUnitOfWork _groupUnitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GroupService(IGroupUnitOfWork groupUnitOfWork, ICurrentUserService currentUserService)
    {
        _groupUnitOfWork = groupUnitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<(IList<Group> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
    {
        var columnsMap = new Dictionary<string, Expression<Func<Group, object>>>()
        {
            ["Name"] = v => v.Name
        };

        var result = await _groupUnitOfWork.GroupRepository.GetAsync<Group>(
            x => x,
            x => (x.Name.Contains(searchText)) && (!userId.HasValue || x.UserId == userId.Value),
            x => x.ApplyOrdering(columnsMap, orderBy),
            null,
            pageIndex, pageSize, true);

        result.Total = await _groupUnitOfWork.GroupRepository.GetCountAsync(x => x.UserId == userId);

        return (result.Items, result.Total, result.TotalFilter);
    }

    public async Task<Group> GetByIdAsync(int id)
    {
        return await _groupUnitOfWork.GroupRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Group entity)
    {
        var isExists = await _groupUnitOfWork.GroupRepository.IsExistsAsync(
            x => x.Name == entity.Name && x.UserId == entity.UserId);

        if (isExists)
            throw new DuplicationException(nameof(entity.Name));

        await _groupUnitOfWork.GroupRepository.AddAsync(entity);
        await _groupUnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActiveStatusAsync(Group entity)
    {
        entity.IsActive = !entity.IsActive;
        await _groupUnitOfWork.GroupRepository.UpdateAsync(entity);
        await _groupUnitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Group entity)
    {
        var isExists = await _groupUnitOfWork.GroupRepository.IsExistsAsync(
            x => x.Name == entity.Name && x.Id != entity.Id && x.UserId == entity.UserId);

        if (isExists)
            throw new DuplicationException(nameof(entity.Name));

        var updateEntity = await GetByIdAsync(entity.Id);
        updateEntity.Name = entity.Name;

        await _groupUnitOfWork.GroupRepository.UpdateAsync(updateEntity);
        await _groupUnitOfWork.SaveChangesAsync();
    }

    public async Task<Group> DeleteAsync(int id)
    {
        var group = await GetByIdAsync(id);
        await _groupUnitOfWork.GroupRepository.DeleteAsync(id);
        await _groupUnitOfWork.SaveChangesAsync();
        return group;
    }

    public async Task<IList<(int Value, string Text, int ContactCount)>> GetAllGroupForSelectAsync(Guid? userId)
    {
        return (await _groupUnitOfWork.GroupRepository.GetAsync(
            x => new { Value = x.Id, Text = x.Name, ContactCount = x.ContactGroups.Count },
            x => !x.IsDeleted && x.IsActive && (!userId.HasValue || x.UserId == userId.Value),
            x => x.OrderBy(o => o.Name),
            x => x.Include(i => i.ContactGroups),
            true))
            .Select(x => (Value: x.Value, Text: x.Text, ContactCount: x.ContactCount))
            .ToList();
    }

    public async Task<int> GetGroupCountAsync(Guid? userId)
    {
        return await _groupUnitOfWork.GroupRepository.GetCountAsync(x => x.UserId == userId);
    }

    public async Task<int> GetGroupCountAsync()
    {
        return await _groupUnitOfWork.GroupRepository.GetCountAsync();
    }

    public void Dispose()
    {
        _groupUnitOfWork?.Dispose();
    }
}


