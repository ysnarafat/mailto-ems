using EmailMarketing.Common.Services;
using EmailMarketing.Membership.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models.Profile
{
    public class ChangePasswordModel : AdminBaseModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Did not match with new password")]
        public string ConfirmNewPassword { get; set; }

        public ChangePasswordModel() : base() { }
        public ChangePasswordModel(ICurrentUserService currentUserService, IApplicationUserService applicationuserService)
            : base(currentUserService, applicationuserService) { }

        internal async Task<bool> ChangeMemberPasswordAsync()
        {
            var result = await _applicationuserService.ChangePasswordAsync(_currentUserService.UserId, CurrentPassword, NewPassword);
            return result;
        }

    }
}
