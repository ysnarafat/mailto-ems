using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ContactsModel : ContactsBaseModel
    {
        public IList<Contact> Contacts { get; set; }

        public ContactsModel(IContactService contactService,
            ICurrentUserService currentUserService) : base(contactService, currentUserService)
        {

        }
        public ContactsModel() : base()
        {

        }
        public async Task<object> GetAllContactAsync(DataTablesAjaxRequestModel tableModel)
        {
            var result = await _contactService.GetAllContactAsync(
                _currentUserService.UserId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Group", "Email" }),
                tableModel.PageIndex, tableModel.PageSize);


            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,
                data = (from item in result.Items
                        select new string[]
                        {
                            string.Join(", ",item.ContactGroups.Select(x=>x.Group.Name)),
                            item.Email,
                            item.Id.ToString()
                        }).ToArray()

            };
        }

        public async Task<string> DeleteAsync(int id)
        {
            var name = await _contactService.DeleteAsync(id);
            return name.Email;
        }
    }
}
