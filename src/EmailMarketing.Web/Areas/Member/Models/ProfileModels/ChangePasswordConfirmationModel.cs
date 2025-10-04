using Microsoft.AspNetCore.Http;

namespace EmailMarketing.Web.Areas.Member.Models.ProfileModels
{
    public class ChangePasswordConfirmationModel : MemberBaseModel
    {
        public ChangePasswordConfirmationModel() : base() { }
        public ChangePasswordConfirmationModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }


    }
}
