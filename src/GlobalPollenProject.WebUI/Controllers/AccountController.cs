using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.WebUI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GlobalPollenProject.WebUI.Controllers
{
    [Authorize]
    [ApiVersionNeutral]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDetails model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model);
                if (!result.IsValid) 
                {
                    ModelState.AddServiceErrors(result.Messages);
                    return View(model);
                }
                return RedirectToLocal(returnUrl);
                // TODO Lockout status
                // TODO Catch all other possibilities as errors
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(NewAppUser model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userService.RegisterForAccount(model);
            if (!result.IsValid)
            {
                ModelState.AddServiceErrors(result.Messages);
                return View(model);
            }

            return RedirectToAction("AwaitingEmailConfirmation");
        }

        [AllowAnonymous]
        public IActionResult AwaitingEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _userService.Logout();            
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public IActionResult ExternalLogin(string provider, string returnUrl = null)
        // {
        //     // Request a redirect to the external login provider.
        //     var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        //     var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //     return new ChallengeResult(provider, properties);
        // }

        // [HttpGet]
        // [AllowAnonymous]
        // public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        // {
        //     var info = await _signInManager.GetExternalLoginInfoAsync();
        //     if (info == null)
        //     {
        //         return RedirectToAction(nameof(Login));
        //     }

        //     // Sign in the user with this external login provider if the user already has a login.
        //     var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        //     if (result.Succeeded)
        //     {
        //         return RedirectToLocal(returnUrl);
        //     }
        //     if (result.RequiresTwoFactor)
        //     {
        //         return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
        //     }
        //     if (result.IsLockedOut)
        //     {
        //         return View("Lockout");
        //     }
        //     else
        //     {
        //         // If the user does not have an account, then ask the user to create an account.
        //         ViewData["ReturnUrl"] = returnUrl;
        //         ViewData["LoginProvider"] = info.LoginProvider;
        //         var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //         return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
        //     }
        // }

        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        // {
        //     if (User.Identity.IsAuthenticated)
        //     {
        //         return RedirectToAction(nameof(ManageController.Index), "Manage");
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         // Get the information about the user from the external login provider
        //         var info = await _signInManager.GetExternalLoginInfoAsync();
        //         if (info == null)
        //         {
        //             return View("ExternalLoginFailure");
        //         }

        //         //Process Im.Acm.Pollen organisation details
        //         var org = _applicationDbContext.Organisations.FirstOrDefault(o => o.Name == model.Organisation);
        //         if (org == null)
        //         {
        //             org = new Organisation()
        //             {
        //                 Name = model.Organisation
        //             };
        //             _applicationDbContext.Organisations.Add(org);
        //             _applicationDbContext.SaveChanges();
        //         }

        //         var user = new AppUser
        //         {
        //             UserName = model.Email,
        //             Email = model.Email,
        //             FirstName = model.FirstName,
        //             LastName = model.LastName,
        //             Title = model.Title,
        //             Organisation = org
        //         };

        //         var result = await _userManager.CreateAsync(user);
        //         if (result.Succeeded)
        //         {
        //             result = await _userManager.AddLoginAsync(user, info);
        //             if (result.Succeeded)
        //             {
        //                 await _signInManager.SignInAsync(user, isPersistent: false);
        //                 return RedirectToLocal(returnUrl);
        //                 //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //                 //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        //                 //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
        //                 //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        //                 //return RedirectToAction("AwaitingEmailConfirmation");
        //             }
        //         }
        //         AddErrors(result);
        //     }

        //     ViewData["ReturnUrl"] = returnUrl;
        //     return View(model);
        // }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userService.RequestValidationEmail(userId, code);
            return View(result.IsValid ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var user = await _userManager.FindByNameAsync(model.Email);
        //         if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //         {
        //             // Don't reveal that the user does not exist or is not confirmed
        //             return View("ForgotPasswordConfirmation");
        //         }

        //         // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        //         // Send an email with this link
        //         var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //         var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        //         await _emailSender.SendEmailAsync(model.Email, "Reset Password",
        //            "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
        //         return View("ForgotPasswordConfirmation");
        //     }

        //     // If we got this far, something failed, redisplay form
        //     return View(model);
        // }

        // [HttpGet]
        // [AllowAnonymous]
        // public IActionResult ForgotPasswordConfirmation()
        // {
        //     return View();
        // }

        // [HttpGet]
        // [AllowAnonymous]
        // public IActionResult ResetPassword(string code = null)
        // {
        //     return code == null ? View("Error") : View();
        // }

        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //     var user = await _userManager.FindByNameAsync(model.Email);
        //     if (user == null)
        //     {
        //         // Don't reveal that the user does not exist
        //         return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //     }
        //     var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //     if (result.Succeeded)
        //     {
        //         return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //     }
        //     AddErrors(result);
        //     return View();
        // }

        // [HttpGet]
        // [AllowAnonymous]
        // public IActionResult ResetPasswordConfirmation()
        // {
        //     return View();
        // }

        // #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        // #endregion
    }
}
