using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Services.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class AddSingleContactModel : ContactsBaseModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsContactExist { get; set; }
        public IList<ContactValueTextModel> GroupSelectList { get; set; }
        public IList<ContactValueTextModel> ContactValueMapsStandard { get; set; }
        public IList<ContactValueTextModel> ContactValueMapsCustom { get; set; }

        public AddSingleContactModel(IContactService contactService,
           ICurrentUserService currentUserService) : base(contactService, currentUserService)
        {

        }
        public AddSingleContactModel() : base()
        {

        }

        public async Task LoadContactInformationAsync()
        {
            GroupSelectList = (await _contactService.GetAllGroupsAsync(_currentUserService.UserId))
                                           .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text, Count = x.Count, IsChecked = false }).ToList();

            ContactValueMapsStandard = (await _contactService.GetAllContactValueMapsStandard())
                                           .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text }).ToList();

            ContactValueMapsCustom = (await _contactService.GetAllContactValueMapsCustom(_currentUserService.UserId))
                                           .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text }).ToList();
        }

        public async Task<bool> IsContactExistAsync()
        {
            var existingContact = await _contactService.IsContactExist(Email, _currentUserService.UserId);
            IsContactExist = true;
            return existingContact;
        }

        public async Task SaveContactAsync()
        {
            if (this.GroupSelectList == null) throw new Exception("Please add/activate atleast one group to add contact.");
            else if (!this.GroupSelectList.Any(x => x.IsChecked)) throw new Exception("Please select at least one group.");

            try
            {
                var newContact = new Contact();
                newContact.ContactValueMaps = new List<ContactValueMap>();
                newContact.ContactGroups = new List<ContactGroup>();
                newContact.CreatedBy = _currentUserService.UserId;
                newContact.Created = DateTime.Now;
                newContact.Email = this.Email;
                newContact.UserId = _currentUserService.UserId;


                #region Add Standard ContactValueMaps
                if (ContactValueMapsStandard.Any())
                {
                    foreach (var item in ContactValueMapsStandard)
                    {
                        if (string.IsNullOrWhiteSpace(item.Input)) continue;

                        var contactValueMap = new ContactValueMap();
                        contactValueMap.Value = item.Input;
                        contactValueMap.FieldMapId = (int)item.Value;
                        newContact.ContactValueMaps.Add(contactValueMap);

                    }
                }
                #endregion

                #region Add Custom ContactValueMaps

                if (ContactValueMapsCustom != null)
                {
                    foreach (var item in ContactValueMapsCustom)
                    {
                        if (string.IsNullOrWhiteSpace(item.Input)) continue;

                        var contactValueMap = new ContactValueMap();
                        contactValueMap.Value = item.Input;
                        contactValueMap.FieldMapId = (int)item.Value;
                        newContact.ContactValueMaps.Add(contactValueMap);
                    }
                }
                #endregion

                #region Add Contact Group

                foreach (var item in GroupSelectList)
                {
                    if (item.IsChecked)
                    {
                        var newContactGroup = new ContactGroup();
                        newContactGroup.GroupId = (int)item.Value;
                        newContact.ContactGroups.Add(newContactGroup);
                    }
                }

                #endregion

                await _contactService.AddContact(newContact);

            }
            catch (Exception)
            {
                throw new Exception("Failed to Add Contact.");
            }

        }

    }
}
