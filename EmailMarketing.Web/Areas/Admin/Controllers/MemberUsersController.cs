using Autofac;
using EmailMarketing.Common.Exceptions;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Web.Areas.Admin.Enums;
using EmailMarketing.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantsUserRoleName.SuperAdminOrAdmin)]
    public class MemberUsersController : Controller
    {
        private readonly ILogger<MemberUsersController> _logger;

        public MemberUsersController(ILogger<MemberUsersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = Startup.AutofacContainer.Resolve<MemberUserModel>();
            return View(model);
        }

        public IActionResult Add()
        {
            var model = new CreateMemberUserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind(nameof(CreateMemberUserModel.FullName),
            nameof(CreateMemberUserModel.Email),
            nameof(CreateMemberUserModel.DateOfBirth),
            nameof(CreateMemberUserModel.Gender),
            nameof(CreateMemberUserModel.Address),
            nameof(CreateMemberUserModel.PhoneNumber))] CreateMemberUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.CreateUserAsync();
                    model.Response = new ResponseModel("Member Added successful.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Member added failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new MemberEditUserModel();
            await model.LoadByIdAsync(id);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind(nameof(MemberEditUserModel.Id),
            nameof(CreateMemberUserModel.DateOfBirth),
            nameof(MemberEditUserModel.Email),
            nameof(MemberEditUserModel.Gender),
            nameof(MemberEditUserModel.Address),
            nameof(MemberEditUserModel.FullName),
            nameof(MemberEditUserModel.PhoneNumber))] MemberEditUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await model.UpdateAsync();
                    model.Response = new ResponseModel("Member edit successful.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (DuplicationException ex)
                {
                    model.Response = new ResponseModel(ex.Message, ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Member edit failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var model = new MemberUserModel();
                try
                {
                    var title = await model.DeleteAsync(id);
                    model.Response = new ResponseModel($"Member {title} successfully deleted.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Member delete failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            if (ModelState.IsValid)
            {
                var model = new MemberUserModel();
                try
                {
                    var user = await model.BlockUnblockAsync(id);
                    model.Response = new ResponseModel($"Member {user.Name} successfully {(user.IsBlocked == true ? "Blocked" : "Unblocked")}.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Block/Unblock Operation failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            if (ModelState.IsValid)
            {
                var model = new MemberUserModel();
                try
                {
                    var title = await model.ResetPasswordAsync(id);
                    model.Response = new ResponseModel($"Member {title} Password Reset Successfully.", ResponseType.Success);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    model.Response = new ResponseModel("Password Reset failured.", ResponseType.Failure);
                    _logger.LogError(ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetUsers()
        {

            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = Startup.AutofacContainer.Resolve<MemberUserModel>();
            var data = await model.GetAllAsync(tableModel);
            return Json(data);
        }

        public async Task<IActionResult> UserInformation(Guid id)
        {
            var model = new MemberUserInformationModel();
            await model.LoadByIdAsync(id);
            return View(model);
        }
    }
}