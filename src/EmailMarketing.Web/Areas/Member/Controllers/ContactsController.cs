using Autofac;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Enums;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Contacts;
using EmailMarketing.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class ContactsController : Controller
    {
        private readonly ILogger<ContactsController> _logger;
        protected readonly IOptions<AppSettings> _appSettings;
        public ContactsController(ILogger<ContactsController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index()
        {
            var model = Startup.AutofacContainer.Resolve<ContactsModel>();
            return View(model);
        }

        public IActionResult ManageUploads()
        {
            var model = new ContactsModel();
            return View(model);
        }

        public IActionResult CustomFields()
        {
            var model = Startup.AutofacContainer.Resolve<FieldMapModel>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddOrEdit(int? id)
        {
            var model = new FieldMapModel();

            #region for edit
            if (id.HasValue && id != 0)
            {
                await model.LoadByIdAsync(id.Value);
            }
            #endregion

            return PartialView("_AddOrEdit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(FieldMapModel model)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id.HasValue && model.Id != 0)
                    {
                        await model.UpdateFieldMapAsync();
                        _logger.LogInformation("Updated custom field.");
                    }
                    else
                    {
                        await model.AddFieldMapAsync();
                        _logger.LogInformation("Added new custom field.");
                    }

                    TempData["SuccessNotify"] = "Field has been successfully saved";
                    return RedirectToAction("CustomFields");

                }
                catch
                {

                    TempData["ErrorNotify"] = "Field Already Exist.";
                    _logger.LogError("Field Already Exist.");
                }
            }
            return RedirectToAction("CustomFields");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateFieldMap(int id)
        {
            var model = new FieldMapModel();
            try
            {
                var customFieldMap = await model.ActivateFieldMapAsync(id);
                model.Response = new ResponseModel($"{customFieldMap.DisplayName} successfully {(customFieldMap.IsActive == true ? "Activated" : "Deactivated")}.", ResponseType.Success);
                _logger.LogInformation("Custome Field Map Active Status updated");
            }
            catch (Exception ex)
            {
                model.Response = new ResponseModel("Active/InActive Operation failured.", ResponseType.Failure);
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("CustomFields");
        }
        public async Task<IActionResult> GetAllFieldMap()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<FieldMapModel>();
            var data = await model.GetAllFieldMapAsync(tableModel);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> AddSingleContact()
        {
            var model = new AddSingleContactModel();
            await model.LoadContactInformationAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSingleContact(AddSingleContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingContact = await model.IsContactExistAsync();
                    if (existingContact == true)
                    {
                        model.Response = new ResponseModel("Contact already exist. You can update the existing contact.", ResponseType.Failure);
                    }
                    else
                    {
                        await model.SaveContactAsync();
                        var msg = "Added Contact Successfully";
                        _logger.LogInformation("Single Contact Added Successfully");
                        model.Response = new ResponseModel(msg, ResponseType.Success);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //var msg = "Failed to Add Contact";
                    model.Response = new ResponseModel("Failed to Add Contact", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            await model.LoadContactInformationAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(int id)
        {
            var model = new EditContactsModel();
            await model.LoadContactByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(EditContactsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingContact = await model.IsContactExistAsync();
                    if (existingContact == true)
                    {
                        model.Response = new ResponseModel("Contact already exist. Please provide another email.", ResponseType.Failure);
                    }
                    else
                    {
                        await model.UpdateAsync();
                        _logger.LogInformation("Contact Successfully Updated.");
                        model.Response = new ResponseModel("Contact Updated", ResponseType.Success);
                        return RedirectToAction("Index");
                    }
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Failed to update Contact", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }
            await model.LoadContactByIdAsync(model.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var model = new ContactsModel();
                try
                {
                    var title = await model.DeleteAsync(id);
                    model.Response = new ResponseModel($"Contact {title} successfully deleted.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Contact delete failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetContacts()
        {

            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<ContactsModel>();
            var data = await model.GetAllContactAsync(tableModel);
            return Json(data);
        }

        public async Task<IActionResult> ContactDetails(int id)
        {
            var model = new ContactDetailsModel();
            await model.LoadByIdAsync(id);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Export()
        {
            var model = new ContactExportModel();
            model.GroupSelectList = await model.GetAllGroupDetailsAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Export(
            ContactExportModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model._appSettings = _appSettings.Value;
                    if (model.IsExportAll)
                    {
                        await model.ExportAllContact();
                    }
                    else
                    {
                        await model.ExportContactsGroupwise();
                    }
                    _logger.LogInformation("Succecssfully Added to DownloadQueue. Waiting to Complete to Export");
                    model.Response = new ResponseModel("Successfully added to queue. Please wait a while to complete the task.", ResponseType.Success);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                }
            }

            model.GroupSelectList = await model.GetAllGroupDetailsAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewContactExportFiles()
        {
            var model = new ContactExportModel();
            //var model = Startup.AutofacContainer.Resolve<ContactExportModel>();
            return View(model);
        }

        public async Task<IActionResult> GetContactExports()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new ContactExportModel();
            var data = await model.GetAllContactExportAsync(tableModel);
            return Json(data);
        }

        public async Task<IActionResult> GetDownloadFile(int id)
        {
            var model = new ContactExportModel();
            try
            {
                var file = await model.GetDownloadedFileByIdAsync(id);
                var path = file.FileUrl;

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.ms-excel", file.FileName);
            }
            catch (Exception)
            {
                model.Response = new ResponseModel("Cannot find file", ResponseType.Failure);
                _logger.LogError("Cannot Find Contact Export File to Download.");
                return RedirectToAction("ViewContactExportFiles");
            }
        }

    }
}
