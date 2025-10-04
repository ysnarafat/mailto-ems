using EmailMarketing.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ContactDetailsModel : ContactsBaseModel
    {
        public List<(bool IsStandard, List<ValueTextModel> Values)> ContactValueMaps { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Groups { get; set; }
        public ContactDetailsModel() : base()
        {

        }

        public async Task LoadByIdAsync(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            this.Id = contact.Id;
            this.Email = contact.Email;
            this.ContactValueMaps = contact.ContactValueMaps.Select(x =>
                new ValueTextModel
                {
                    Value = x.Value,
                    Text = x.FieldMap.DisplayName,
                    IsChecked = x.FieldMap.IsStandard
                }).GroupBy(x => x.IsChecked).Select(x =>
                (IsStandard: x.Key, Values: x.ToList())).ToList();
            this.Groups = string.Join(", ", contact.ContactGroups.Select(x => x.Group.Name));
        }
    }
}
