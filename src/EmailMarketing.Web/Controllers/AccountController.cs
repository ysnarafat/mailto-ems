using EmailMarketing.Common.Services;
using EmailMarketing.Membership.Constants;
using EmailMarketing.Membership.Entities;
using EmailMarketing.Membership.Services;
using EmailMarketing.Membership.Templates;
using EmailMarketing.Web.Core;
using EmailMarketing.Web.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EmailMarketing.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMailerService _mailerService;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;

        public AccountController(ApplicationSignInManager signInManager,
            ILogger<AccountController> logger,
            ApplicationUserManager userManager,
            IEmailSender emailSender,
            IMailerService mailerService,
            IWebHostEnvironment env,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _mailerService = mailerService;
            _env = env;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");

                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ViewData["ExternalLogins"] = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ViewData["ReturnUrl"] = returnUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get login page and Error message: {ex.Message}");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }

                    var isValidUser = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (isValidUser)
                    {
                        if (user.IsBlocked)
                        {
                            ModelState.AddModelError(string.Empty, "You have been blocked !!");
                        }
                        else if (!(await _userManager.IsEmailConfirmedAsync(user)))
                        {
                            ModelState.AddModelError(string.Empty, "Please confirm your email account.");
                        }
                        else if (user.PasswordChangedCount == 0)
                        {
                            return RedirectToAction("ChangeDefaultPassword", new { email = user.Email });
                        }
                        else
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles == null || roles.Count == 0)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            }

                            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            }
                            else if (roles.Any(x => x.Equals(ConstantsValue.UserRoleName.Member)))
                            {
                                _logger.LogInformation("User logged in.");
                                return RedirectToAction(nameof(Index), "Dashboard", new { Area = "Member" });
                            }
                            else if (roles.Any(x => x.Equals(ConstantsValue.UserRoleName.SuperAdmin) || x.Equals(ConstantsValue.UserRoleName.Admin)))
                            {
                                _logger.LogInformation("Admin logged in.");
                                return RedirectToAction(nameof(Index), "Dashboard", new { Area = "Admin" });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to login for: {model.Email} and Error message: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
            }

            // If we got this far, something failed, redisplay form
            //return Page();
            return View(model);
        }

        public async Task<IActionResult> Register(string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;

                ViewData["ExternalLogins"] = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get register page and Error message: {ex.Message}");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
                ViewData["ExternalLogins"] = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, PasswordChangedCount = 1 };
                            var result = await _userManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                _logger.LogInformation("User created a new account with password.");

                                await _userManager.AddToRoleAsync(user, ConstantsValue.UserRoleName.Member);
                                _logger.LogInformation("Role --Member-- assined to User");

                                //Email Verification Section
                                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                                //Activate Email Link
                                var emailVerificationlink = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

                                var subject = "Confirm Account Registration";

                                //Email Sending
                                var accountConfirmationTemplate = new AccountConfirmationEmailTemplate(user.FullName,
                                    emailVerificationlink, _appSettings.CompanyFullName, _appSettings.CompanyShortName,
                                    _appSettings.CompanyWebsiteUrl);
                                var emailBody = accountConfirmationTemplate.TransformText();
                                _logger.LogInformation("Email body generated");

                                await _mailerService.SendEmailAsync(user.Email, subject, emailBody);
                                _logger.LogInformation("Email sent");

                                scope.Complete();

                                return RedirectToAction("EmailVerificationConfirmation");
                            }
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            _logger.LogError(ex, $"Failed to register for: {model.Email} and Error message: {ex.Message}");
                            throw ex;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to register for: {model.Email} and Error message: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
            }

            // If we got this far, something failed, redisplay form
            //return Page();

            return View(model);
        }

        public async Task<IActionResult> AccessDenied(string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;

                ViewData["ExternalLogins"] = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get access denied page and Error message: {ex.Message}");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToAction("ForgotPasswordConfirmation");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action(nameof(ResetPassword), "Account", new { email = model.Email, code },
                                                        Request.Scheme, Request.Host.ToString());

                    var subject = "Recover Account Password";

                    var passwordResetTemplate = new PasswordResetEmailTemplate(user.FullName,
                                    passwordResetLink, _appSettings.CompanyFullName, _appSettings.CompanyShortName,
                                    _appSettings.CompanyWebsiteUrl);
                    var emailBody = passwordResetTemplate.TransformText();

                    await _mailerService.SendEmailAsync(user.Email, subject, emailBody);

                    return RedirectToAction("ForgotPasswordConfirmation");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to forgot password for: {model.Email} and Error message: {ex.Message}");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string code)
        {
            if (email == null || code == null)
            {
                ModelState.AddModelError("", "Invalid Password Reset Token");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    user.PasswordChangedCount += 1;
                    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        var updatedResult = await _userManager.UpdateAsync(user);
                        if (updatedResult.Succeeded)
                        {
                            return RedirectToAction("ResetPasswordConfirmation");
                        }
                        //ModelState.AddModelError(String.Empty, "Something went wrong. Please Try again");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to reset password for: {model.Email} and Error message: {ex.Message}");
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult EmailVerificationConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", new { returnUrl });
            }

        }


        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null) return BadRequest();

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;
                    var updatedResult = await _userManager.UpdateAsync(user);

                    if (updatedResult.Succeeded)
                    {
                        return View();
                    }

                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to verify email for (user id): {userId} and Error message: {ex.Message}");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult ChangeDefaultPassword(string email)
        {
            var model = new ChangeDefaultPasswordViewModel();
            model.Email = email;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeDefaultPassword(ChangeDefaultPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                    if (result.Succeeded)
                    {
                        user.PasswordChangedCount = 1;
                        var updatedResult = await _userManager.UpdateAsync(user);

                        if (updatedResult.Succeeded)
                        {
                            return RedirectToAction("ChangePasswordConfirmation");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Please Insert Correct Password");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to change default password for: {model.Email} and Error message: {ex.Message}");
            }

            return View(model);
        }

        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }


    }
}
