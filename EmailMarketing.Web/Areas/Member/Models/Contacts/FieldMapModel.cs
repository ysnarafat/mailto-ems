using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class FieldMapModel : ContactsBaseModel
    {
        public int? Id { get; set; }
        public Guid? UserId { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public bool IsStandard { get; set; }
        public FieldMapModel(IFieldMapService fieldMapService,
            ICurrentUserService currentUserService) : base(fieldMapService, currentUserService)
        {

        }
        public FieldMapModel() : base()
        {

        }
        public async Task<object> GetAllFieldMapAsync(DataTablesAjaxRequestModel tableModel)
        {
            var result = await _fieldMapService.GetAllAsync(
                _currentUserService.UserId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "DisplayName" }),
                tableModel.PageIndex, tableModel.PageSize);


            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            item.DisplayName,
                            item.IsActive ? "Yes" : "No",
                            item.Id.ToString()
                        }).ToArray()

            };
        }

        public async Task<FieldMap> ActivateFieldMapAsync(int id)
        {
            var customFieldMap = await _fieldMapService.ActivateUpdateAsync(id);
            return customFieldMap;
        }

        public async Task AddFieldMapAsync()
        {

            var entity = new FieldMap
            {
                DisplayName = this.DisplayName,
                UserId = _currentUserService.UserId,
                IsStandard = false
            };
            await _fieldMapService.AddAsync(entity);
        }

        public async Task LoadByIdAsync(int id)
        {
            var result = await _fieldMapService.GetByIdAsync(id);
            this.Id = result.Id;
            this.UserId = result.UserId;
            this.DisplayName = result.DisplayName;
            this.IsStandard = result.IsStandard;
        }

        public async Task UpdateFieldMapAsync()
        {
            var entity = new FieldMap
            {
                Id = this.Id.Value,
                DisplayName = this.DisplayName,
                UserId = _currentUserService.UserId,
                IsStandard = false
            };
            await _fieldMapService.UpdateAsync(entity);
        }
    }
}
