using Autofac;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Core;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models.AdminUsers
{
    public class CreateAdminUsersModel : AdminBaseModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM, yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        private readonly IApplicationUserService _applicationUserService;
        private readonly AppSettings _appSettings;

        public CreateAdminUsersModel()
        {
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
            _appSettings = Startup.AutofacContainer.Resolve<IOptions<AppSettings>>().Value;
        }

        public CreateAdminUsersModel(IApplicationUserService applicationUserService, IOptions<AppSettings> appSettings)
        {
            _applicationUserService = applicationUserService;
            _appSettings = appSettings.Value;
        }

        public async Task CreateAdminAsync()
        {
            var user = new ApplicationUser
            {
                FullName = this.FullName,
                UserName = this.Email,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Gender = this.Gender,
                DateOfBirth = this.DateOfBirth,
                Address = this.Address,
                EmailConfirmed = true
            };

            await _applicationUserService.AddAsync(user, ConstantsValue.UserRoleName.Admin, _appSettings.UserDefaultPassword);
        }
    }
}
