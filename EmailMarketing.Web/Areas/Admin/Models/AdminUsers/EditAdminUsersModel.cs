using Autofac;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models.AdminUsers
{
    public class EditAdminUsersModel : AdminBaseModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM, yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        private readonly IApplicationUserService _applicationUserService;

        public EditAdminUsersModel()
        {
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
        }

        public EditAdminUsersModel(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task LoadByIdAsync(Guid id)
        {
            var user = await _applicationUserService.GetByIdAsync(id);
            this.Id = user.Id;
            this.FullName = user.FullName;
            this.Email = user.Email;
            this.DateOfBirth = user.DateOfBirth;
            this.PhoneNumber = user.PhoneNumber;
            this.Address = user.Address;
            this.Gender = user.Gender;
        }

        public async Task UpdateAsync()
        {
            var user = new ApplicationUser
            {
                Id = this.Id,
                FullName = this.FullName,
                UserName = this.Email,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Gender = this.Gender,
                DateOfBirth = this.DateOfBirth,
                Address = this.Address
            };

            await _applicationUserService.UpdateAsync(user);
        }
    }
}
