using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Services.Groups;
using EmailMarketing.Membership.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Groups
{

    public class GroupModel : GroupBaseModel
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public GroupModel(IGroupService groupService, IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService) : base(groupService, applicationUserService, currentUserService) { }
        public GroupModel() : base() { }

        public async Task<Group> ActivateGroupAsync(int id)
        {
            var result = await _groupService.GetByIdAsync(id);
            await _groupService.UpdateActiveStatusAsync(result);
            return result;
        }
        public async Task<object> GetAllAsync(DataTablesAjaxRequestModel tableModel)
        {
            var userId = _currentUserService.UserId;
            var result = await _groupService.GetAllAsync(
                userId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Name" }),
                tableModel.PageIndex, tableModel.PageSize);
            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,

                data = (from item in result.Items
                        select new string[]
                        {
                                    item.Name,
                                    item.IsActive ? "Yes" : "No",
                                    item.Id.ToString()
                        }
                        ).ToArray()

            };
        }

        public async Task<string> DeleteAsync(int id)
        {
            var group = await _groupService.DeleteAsync(id);
            return group.Name;
        }

        public async Task AddAsync()
        {

            var entity = new Group
            {
                Name = this.Name,
                UserId = _currentUserService.UserId
            };
            await _groupService.AddAsync(entity);
        }

        public async Task LoadByIdAsync(int id)
        {
            var result = await _groupService.GetByIdAsync(id);
            this.Id = result.Id;
            this.Name = result.Name;
        }

        public async Task UpdateAsync()
        {
            var entity = new Group
            {
                Id = this.Id.Value,
                Name = this.Name,
                UserId = _currentUserService.UserId
            };
            await _groupService.UpdateAsync(entity);
        }
    }
}
