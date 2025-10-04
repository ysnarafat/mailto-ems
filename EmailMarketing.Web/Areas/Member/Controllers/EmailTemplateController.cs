using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Campaigns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class EmailTemplateController : Controller
    {
        private readonly ILogger<EmailTemplateController> _logger;

        public EmailTemplateController(ILogger<EmailTemplateController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewEmailTemplates()
        {
            var model = new EmailTemplateModel();
            model.EmailTemplateList = await model.GetTemplateByUserIDAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddEmailTemplate()
        {
            var model = new CreateEmailTemplateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmailTemplateAsync([Bind(nameof(CreateEmailTemplateModel.EmailTemplateBody),
            nameof(CreateEmailTemplateModel.EmailTemplateName),
            nameof(CreateEmailTemplateModel.IsPersonalized))] CreateEmailTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.CreateEmailTemplate();
                    _logger.LogInformation("Email Template Added Successfully");
                    model.Response = new ResponseModel("Template Added Successfully", Enums.ResponseType.Success);
                    return RedirectToAction("AddEmailTemplate");
                }
                catch (DuplicationException ex)
                {
                    _logger.LogError(ex.Message);
                    model.Response = new ResponseModel(ex.Message, Enums.ResponseType.Failure);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    model.Response = new ResponseModel("Template Creation Failed", Enums.ResponseType.Failure);
                }
            }
            return View(model);
        }
    }
}
