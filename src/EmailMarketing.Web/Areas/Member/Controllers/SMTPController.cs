using Autofac;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Enums;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class SMTPController : Controller
    {
        private readonly ILogger<SMTPController> _logger;

        public SMTPController(ILogger<SMTPController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = Startup.AutofacContainer.Resolve<SMTPModel>();
            return View(model);
        }
        public IActionResult Add()
        {
            var model = new CreateSMTPModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind(nameof(CreateSMTPModel.Server),
            nameof(CreateSMTPModel.Port),
            nameof(CreateSMTPModel.SenderName),
            nameof(CreateSMTPModel.SenderEmail),
            nameof(CreateSMTPModel.UserName),
            nameof(CreateSMTPModel.Password),
            nameof(CreateSMTPModel.EnableSSL))] CreateSMTPModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.AddAsync();
                    model.Response = new ResponseModel("SMTP creation successful.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                }
                catch (Exception)
                {
                    model.Response = new ResponseModel("SMTP creation failured.", ResponseType.Failure);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TestSmtp(CreateSMTPModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await model.SmtpTest();
                    return Json(result);
                }
                catch (Exception)
                {
                    return Json(false);
                }
            }
            return Json("failed");

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new EditSMTPModel();
            await model.LoadByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind(nameof(EditSMTPModel.Id),
            nameof(CreateSMTPModel.Server),
            nameof(CreateSMTPModel.Port),
            nameof(CreateSMTPModel.SenderName),
            nameof(CreateSMTPModel.SenderEmail),
            nameof(CreateSMTPModel.UserName),
            nameof(CreateSMTPModel.Password),
            nameof(CreateSMTPModel.EnableSSL))] EditSMTPModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.UpdateAsync();
                    model.Response = new ResponseModel("SMTP edit successful.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                }
                catch (Exception)
                {
                    model.Response = new ResponseModel("SMTP edit failured.", ResponseType.Failure);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateSmtp(Guid id)
        {
            var model = new SMTPModel();
            try
            {
                var smtp = await model.ActivateSmtpAsync(id);
                model.Response = new ResponseModel($"{smtp.Server} successfully {(smtp.IsActive == true ? "Activated" : "Deactivated")}.", ResponseType.Success);
                _logger.LogInformation("Custome Field Map Active Status updated");
            }
            catch (Exception ex)
            {
                model.Response = new ResponseModel("Active/InActive Operation failured.", ResponseType.Failure);
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetSMTP()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<SMTPModel>();
            var data = await model.GetAllAsync(tableModel);
            return Json(data);
        }

    }
}
