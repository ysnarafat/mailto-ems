using Autofac;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Enums;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class ContactUploadController : Controller
    {
        private readonly ILogger<ContactUploadController> _logger;

        public ContactUploadController(ILogger<ContactUploadController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = Startup.AutofacContainer.Resolve<ContactUploadModel>();
            return View(model);
        }

        public async Task<IActionResult> UploadContact()
        {
            var model = new CreateContactUploadModel();
            return View(model);
        }
        public async Task<IActionResult> ViewUploadContact(int id)
        {
            var model = Startup.AutofacContainer.Resolve<ViewContactUploadModel>();
            await model.GetContactUploadData(id);
            await model.SetContactUploadId(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadContact(CreateContactUploadModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.SaveContactsUploadAsync();
                    model.Response = new ResponseModel("Contacts Upload successful. Contacts are currently being processed! This could take a few minutes .In the meantime you can continue working in MailTo.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //model.Response = new ResponseModel("Contacts Upload failured.", ResponseType.Failure);
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinishUpload(int id)
        {
            var model = new ContactUploadModel();
            try
            {
                var result = await model.FinishUploadAsync(id);
                model.Response = new ResponseModel($"{result.FileName} {(result.IsProcessing == true ? "successfully  Finished" : "in Processing")}.", ResponseType.Success);
                _logger.LogInformation($"ConatactUpload - {result.FileName} - Processing Status updated");
            }
            catch (Exception ex)
            {
                model.Response = new ResponseModel("ConatactUpload Processing Status Operation failured.", ResponseType.Failure);
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Index");
        }


        #region json helper method
        public async Task<JsonResult> GetAllFieldMaps()
        {
            var model = new CreateContactUploadModel();
            var data = await model.GetAllFieldMapForSelectAsync();
            return Json(data);
        }

        public async Task<JsonResult> GetAllGroups()
        {
            var model = new CreateContactUploadModel();
            var data = await model.GetAllGroupForSelectAsync();
            return Json(data);
        }

        public async Task<IActionResult> GetContactUploads()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<ContactUploadModel>();
            var data = await model.GetAllAsync(tableModel);
            return Json(data);
        }

        public async Task<IActionResult> GetAllContact()
        {
            var contactUploadId = Convert.ToInt32(Request.Query["contactUploadId"]);
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<ViewContactUploadModel>();
            var data = await model.GetContactByContactUploadIdAsync(tableModel, contactUploadId);
            return Json(data);
        }
        #endregion
    }
}
