using EmailMarketing.Framework.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Contacts
{
    public interface IFieldMapService : IDisposable
    {
        Task<(IList<FieldMap> Items, int Total, int TotalFilter)> GetAllAsync(
            Guid? userId,
            string searchText,
            string orderBy,
            int pageIndex,
            int pageSize);
        Task AddAsync(FieldMap entity);
        Task<FieldMap> GetByIdAsync(int id);
        Task UpdateAsync(FieldMap entity);
        Task<FieldMap> ActivateUpdateAsync(int id);
    }
}
