using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Models.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            var model = new ResourcesModel();
            return View(model);
        }

        public IActionResult ImportContact()
        {
            var model = new ResourcesModel();
            return View(model);
        }
        public IActionResult SendMail()
        {
            var model = new ResourcesModel();
            return View(model);
        }

        public IActionResult ChoseEmailTemplate()
        {
            var model = new ResourcesModel();
            return View(model);
        }
        public IActionResult HelpfulTips()
        {
            var model = new ResourcesModel();
            return View(model);
        }
        public IActionResult CreateXlsxFile()
        {
            var model = new ResourcesModel();
            return View(model);
        }

    }
}