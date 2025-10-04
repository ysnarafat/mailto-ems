using EmailMarketing.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailMarketing.Membership.Services
{
    public interface IApplicationUserService
    {
        Task<(IList<ApplicationUser> Items, int Total, int TotalFilter)> GetAllAsync(
            string searchText,
            string orderBy,
            int pageIndex,
            int pageSize);
        Task<(IList<ApplicationUser> Items, int Total, int TotalFilter)> GetAllAdminAsync(
             string searchText,
             string orderBy,
             int pageIndex,
             int pageSize);
        Task<(IList<ApplicationUser> Items, int Total, int TotalFilter)> GetAllMemberAsync(
            string searchText,
            string orderBy,
            int pageIndex,
            int pageSize);
        Task<int> GetAllMembersAsync();
        Task<ApplicationUser> GetByIdAsync(Guid id);
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        Task<Guid> AddAsync(ApplicationUser entity, Guid userRoleId, string newPassword);
        Task<Guid> AddAsync(ApplicationUser entity, string userRoleName, string newPassword);
        Task<Guid> UpdateAsync(ApplicationUser entity, Guid userRoleId);
        Task<Guid> UpdateAsync(ApplicationUser entity);
        Task<string> DeleteAsync(Guid id);
        Task<(string Name, bool IsActive)> ActiveInactiveAsync(Guid id);
        Task<(string Name, bool IsBlocked)> BlockUnblockAsync(Guid id);
        Task<string> ResetPasswordAsync(Guid id, string newPassword);
        Task<IList<(Guid Value, string Text)>> GetAllForSelectAsync();
        Task<bool> IsExistsUserNameAsync(string name, Guid id);
        Task<bool> IsExistsEmailAsync(string email, Guid id);
        Task<bool> ChangePasswordAsync(Guid id, string CurrentPassword, string NewPassword);
    }
}
