using Autofac;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models
{
    public class MemberEditUserModel : AdminBaseModel
    {
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM, yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        private readonly IApplicationUserService _applicationUserService;

        public MemberEditUserModel()
        {
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
        }
        public MemberEditUserModel(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;

        }

        public async Task LoadByIdAsync(Guid id)
        {
            var user = await _applicationUserService.GetByIdAsync(id);
            this.Id = user.Id;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.FullName = user.FullName;
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
                DateOfBirth = this.DateOfBirth,
                PhoneNumber = this.PhoneNumber,
                Gender = this.Gender,
                Address = this.Address
            };

            await _applicationUserService.UpdateAsync(user);
        }
    }
}
