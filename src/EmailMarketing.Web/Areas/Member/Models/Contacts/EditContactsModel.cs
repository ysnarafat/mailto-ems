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
    public class EditContactsModel : ContactsBaseModel
    {
        [Required]
        public string Email { get; set; }
        public int Id { get; set; }
        public bool IsContactExist { get; set; }
        public List<ContactValueTextModel> GroupSelectList { get; set; }
        public List<ContactValueTextModel> ContactValueMaps { get; set; }
        public List<ContactValueTextModel> ContactValueMapsSelected { get; set; }
        public List<ContactValueTextModel> ContactValueMapsCustomSelected { get; set; }
        public List<ContactValueTextModel> ContactValueMapsCustom { get; set; }

        public EditContactsModel(IContactService contactService,
            ICurrentUserService currentUserService) : base(contactService, currentUserService)
        {

        }

        public EditContactsModel() : base()
        {

        }
        public async Task<bool> IsContactExistAsync()
        {
            var existingContact = await _contactService.IsContactExist(Email, _currentUserService.UserId);
            IsContactExist = true;
            return existingContact;
        }
        public async Task LoadContactByIdAsync(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            this.Id = contact.Id;
            this.Email = contact.Email;

            ContactValueMapsCustom = (await _contactService.GetAllContactValueMapsCustom(_currentUserService.UserId))
                                          .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text }).ToList();

            var SelectedCustomFields = (await _contactService.GetAllContactValueMapsCustom(_currentUserService.UserId, contact.Id))
                                           .Select(x => new ContactValueTextModel { Id = x.Id, Value = x.Value, Text = x.Text, Input = x.Input }).ToList();

            foreach (var item in ContactValueMapsCustom)
            {
                if (!SelectedCustomFields.Any(x => x.Value.Equals(item.Value)))
                {
                    SelectedCustomFields.Add(
                    new ContactValueTextModel
                    {
                        Id = item.Id,
                        Input = item.Input,
                        Value = item.Value,
                        IsChecked = item.IsChecked,
                        Text = item.Text
                    });
                }
            }
            this.ContactValueMapsCustomSelected = SelectedCustomFields;

            ContactValueMaps = (await _contactService.GetAllContactValueMapsStandard())
                                           .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text }).ToList();

            var SelectedStandardFields = (await _contactService.GetAllSelectedContactValueMapsStandard(contact.Id))
                                            .Select(x => new ContactValueTextModel { Id = x.Id, Value = x.Value, Text = x.Text, Input = x.Input }).ToList();

            foreach (var item in ContactValueMaps)
            {
                if (!SelectedStandardFields.Any(x => x.Value.Equals(item.Value)))
                {
                    SelectedStandardFields.Add(
                    new ContactValueTextModel
                    {
                        Id = item.Id,
                        Input = item.Input,
                        Value = item.Value,
                        IsChecked = item.IsChecked,
                        Text = item.Text
                    });
                }
            }
            this.ContactValueMapsSelected = SelectedStandardFields;


            var allGroups = (await _contactService.GetAllGroupsAsync(_currentUserService.UserId))
                                          .Select(x => new ContactValueTextModel { Value = x.Value, Text = x.Text, Count = x.Count, IsChecked = false }).ToList();

            var isSelectedGroups = new HashSet<int>(contact.ContactGroups.Select(x => x.GroupId));

            List<ContactValueTextModel> GroupList = new List<ContactValueTextModel>();

            foreach (var group in allGroups)
            {
                GroupList.Add(
                    new ContactValueTextModel
                    {
                        IsChecked = isSelectedGroups.Contains(group.Value),
                        Text = group.Text,
                        Count = group.Count,
                        Value = group.Value
                    });
            }
            GroupSelectList = GroupList;

        }

        public async Task UpdateAsync()
        {
            if (await _contactService.IsContactExist(Email, _currentUserService.UserId)) throw new Exception("Email Already exist");
            if (!this.GroupSelectList.Any(x => x.IsChecked)) throw new Exception("Please select at least one group.");

            try
            {
                var updatedContact = new Contact
                {
                    Email = this.Email,
                    Id = this.Id,
                    LastModified = DateTime.Now,
                    LastModifiedBy = _currentUserService.UserId
                };

                updatedContact.ContactGroups = new List<ContactGroup>();
                updatedContact.ContactValueMaps = new List<ContactValueMap>();


                var contactValueMaps = new List<ContactValueMap>();

                #region AddConatacValueMapsStandard
                foreach (var item in ContactValueMapsSelected)
                {
                    if (string.IsNullOrWhiteSpace(item.Input)) continue;

                    var contactValueMap = new ContactValueMap();
                    contactValueMap.ContactId = this.Id;
                    contactValueMap.Value = item.Input;
                    contactValueMap.FieldMapId = (int)item.Value;
                    //contactValueMaps.Add(contactValueMap);
                    updatedContact.ContactValueMaps.Add(contactValueMap);
                }
                #endregion

                #region AddContactValueMapsCustom
                foreach (var item in ContactValueMapsCustomSelected)
                {
                    if (string.IsNullOrWhiteSpace(item.Input)) continue;

                    var contactValueMap = new ContactValueMap();
                    contactValueMap.ContactId = this.Id;
                    contactValueMap.Value = item.Input;
                    contactValueMap.FieldMapId = (int)item.Value;
                    //contactValueMaps.Add(contactValueMap);
                    updatedContact.ContactValueMaps.Add(contactValueMap);
                }
                #endregion

                var groupList = new List<ContactGroup>();
                foreach (var item in GroupSelectList)
                {
                    if (item.IsChecked)
                    {
                        var contactGroup = new ContactGroup();
                        contactGroup.ContactId = this.Id;
                        contactGroup.GroupId = item.Value;
                        //groupList.Add(contactGroup);
                        updatedContact.ContactGroups.Add(contactGroup);
                    }
                }

                await _contactService.UpdateAsync(updatedContact);

            }

            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
