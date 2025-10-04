using EmailMarketing.Common.Services;
using EmailMarketing.Membership.Services;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.ProfileModels
{
    public class ProfileInformationModel : MemberBaseModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }

        public ProfileInformationModel() : base() { }
        public ProfileInformationModel(ICurrentUserService currentUserService, IApplicationUserService applicationuserService)
            : base(currentUserService, applicationuserService) { }

        internal async Task LoadInfo()
        {
            var user = await _applicationuserService.GetByIdAsync(_currentUserService.UserId);
            if (user != null)
            {
                FullName = user.FullName;
                PhoneNumber = user.PhoneNumber;
                Address = user.Address;
                DateOfBirth = user.DateOfBirth;
                Email = user.Email;
                Gender = user.Gender;
            }
        }



    }
}
