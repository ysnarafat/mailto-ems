using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

namespace EmailMarketing.Modules.Contacts.Services;

public interface IFieldMapService : IDisposable
{
    Task<(IList<FieldMap> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize);
    Task AddAsync(FieldMap entity);
    Task<FieldMap> GetByIdAsync(int id);
    Task UpdateAsync(FieldMap entity);
    Task<FieldMap> ActivateUpdateAsync(int id);
}

