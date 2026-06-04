using Autofac;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Member.Enums;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize(Roles = ConstantsUserRoleName.Member)]
    public class GroupsController : Controller
    {
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(ILogger<GroupsController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = Startup.AutofacContainer.Resolve<GroupModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateGroup(int id)
        {
            var model = new GroupModel();
            try
            {
                var group = await model.ActivateGroupAsync(id);
                model.Response = new ResponseModel($"{group.Name} successfully {(group.IsActive == true ? "Activated" : "Deactivated")}.", ResponseType.Success);
                _logger.LogInformation("Group Active Status updated");
            }
            catch (Exception ex)
            {
                model.Response = new ResponseModel("Active/InActive Operation failured.", ResponseType.Failure);
                _logger.LogError(ex.Message);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> AddOrEdit(int? id)
        {
            var model = new GroupModel();

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
        public async Task<IActionResult> AddOrEdit(GroupModel model)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                try
                {

                    if (model.Id.HasValue && model.Id != 0)
                    {
                        await model.UpdateAsync();
                        model.Response = new ResponseModel("Group Updated successfully.", ResponseType.Success);
                        _logger.LogInformation($"Group: {model.Name} updated successfully.");
                    }

                    else
                    {
                        await model.AddAsync();
                        model.Response = new ResponseModel("Group creation successful.", ResponseType.Success);
                        _logger.LogInformation($"Group: {model.Name} added successfully.");
                    }

                    return RedirectToAction("Index");
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                    _logger.LogError($"Group '{model.Name}' already exist.");
                }
                catch (Exception)
                {
                    model.Response = new ResponseModel("Group creation failured.", ResponseType.Failure);
                    _logger.LogError($"Failed to create Group '{model.Name}'");
                }
                return RedirectToAction("Index");
            }


            model.Response = new ResponseModel("Group edit failured.", ResponseType.Failure);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new GroupModel();
            await model.DeleteAsync(id);
            return Json(true);
        }
        public async Task<IActionResult> GetGroups()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<GroupModel>();
            var data = await model.GetAllAsync(tableModel);
            return Json(data);
        }

    }
}
