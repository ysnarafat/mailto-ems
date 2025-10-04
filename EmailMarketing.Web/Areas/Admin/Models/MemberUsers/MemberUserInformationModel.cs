using Autofac;
using EmailMarketing.Membership.Services;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Models
{
    public class MemberUserInformationModel : AdminBaseModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }

        private readonly IApplicationUserService _applicationUserService;

        public MemberUserInformationModel()
        {
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
        }
        public MemberUserInformationModel(IApplicationUserService applicationUserService)
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
            this.DateOfBirth = user.DateOfBirth;
            this.EmailConfirmed = user.EmailConfirmed;
            this.IsBlocked = user.IsBlocked;

        }
    }
}
