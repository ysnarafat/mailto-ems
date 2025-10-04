using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IContactUploadService _contactExcelService;

        public DashboardController(ILogger<DashboardController> logger, IWebHostEnvironment env, IContactUploadService contactExcelService)
        {
            _logger = logger;
            _env = env;
            _contactExcelService = contactExcelService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardModel();
            await model.LoadDashboardData();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}