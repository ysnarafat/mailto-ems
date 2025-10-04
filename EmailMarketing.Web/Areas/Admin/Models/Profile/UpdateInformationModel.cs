using EmailMarketing.Common.Services;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models.Profile
{
    public class UpdateInformationModel : AdminBaseModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }

        public UpdateInformationModel() : base() { }
        public UpdateInformationModel(ICurrentUserService currentUserService, IApplicationUserService applicationuserService)
            : base(currentUserService, applicationuserService) { }
        internal async Task Load()
        {
            var user = await _applicationuserService.GetByIdAsync(_currentUserService.UserId);
            if (user != null)
            {
                FullName = user.FullName;
                Email = user.Email;
                DateOfBirth = user.DateOfBirth;
                Gender = user.Gender;
                PhoneNumber = user.PhoneNumber;
                Address = user.Address;
            }
        }
        internal void GetModelData(ApplicationUser user)
        {
            user.FullName = FullName;
            user.Email = Email;
            user.DateOfBirth = DateOfBirth;
            user.Gender = Gender;
            user.PhoneNumber = PhoneNumber;
            user.Address = Address;
        }
        internal async Task UpdateMemberAsync()
        {

            var user = await _applicationuserService.GetByIdAsync(_currentUserService.UserId);
            GetModelData(user);
            await _applicationuserService.UpdateAsync(user);
        }
    }
}
