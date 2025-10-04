using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Models.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class UpgrateMembershipController : Controller
    {
        public IActionResult Index()
        {
            var model = new UpgrateMembershipModel();
            return View(model);
        }
    }
}