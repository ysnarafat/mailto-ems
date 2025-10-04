using EmailMarketing.Framework.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Framework.Services.Groups
{
    public interface IGroupService : IDisposable
    {
        Task<(IList<Group> Items, int Total, int TotalFilter)> GetAllAsync(
            Guid? userId,
            string searchText,
            string orderBy,
            int pageIndex,
            int pageSize);

        Task<Group> GetByIdAsync(int id);
        Task AddAsync(Group entity);
        Task UpdateActiveStatusAsync(Group entity);
        Task UpdateAsync(Group entity);
        Task<Group> DeleteAsync(int id);
        Task<IList<(int Value, string Text, int ContactCount)>> GetAllGroupForSelectAsync(Guid? userId);
        Task<int> GetGroupCountAsync(Guid? userId);
        Task<int> GetGroupCountAsync();
    }
}
