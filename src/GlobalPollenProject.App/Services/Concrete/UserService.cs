using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalPollenProject.App.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<AppServiceResult> Login(LoginDetails user)
        {
            var result = new AppServiceResult();
            var existing = await _userManager.FindByNameAsync(user.Email);
            if (existing != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(existing))
                {
                    result.AddError(string.Empty, "You must have a confirmed email to log in.", AppServiceMessageType.Error);
                    return result;
                }
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: false);
            return result;
        }

        public async Task<AppServiceResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new AppServiceResult();
        }

        public async Task<AppServiceResult<AppUser>> RegisterForAccount(NewAppUser user)
        {
            var result = new AppServiceResult<AppUser>();
            // TODO Automatic organisation creation has been removed. Must be replaced by more robust, seperate use case.
            var entity = new User(user.Title, user.FirstName, user.LastName);
            entity.Email = user.Email;

            var registrationResult = await _userManager.CreateAsync(entity, user.Password);
            if (!registrationResult.Succeeded)
            {
                foreach (var error in registrationResult.Errors)
                {
                    result.AddError(error.Code, error.Description, AppServiceMessageType.Error);
                    return result;
                }
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(entity);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var callbackUrl = "";
            await _emailSender.SendEmailAsync(entity.Email, "Confirm your account",
                "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">" + callbackUrl + "</a>. You can also copy and paste the address into your browser.");
            await _signInManager.SignInAsync(entity, isPersistent: false);

            var appUser = new AppUser()
            {
                Title = entity.Title,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email
            };
            result.AddResult(appUser);
            return result;
        }

        public async Task<AppServiceResult<AppUser>> GetUser(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var appUser = new AppUser()
            {
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            var result = new AppServiceResult<AppUser>();
            result.AddResult(appUser);
            return result;
        }

        public async Task<AppServiceResult> RequestValidationEmail(string userId, string code)
        {
            var result = new AppServiceResult();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                result.AddError("", "Cannot send validation email.", AppServiceMessageType.Error);
                return result;
            }

            var emailResult = await _userManager.ConfirmEmailAsync(user, code);
            if (!emailResult.Succeeded)
            {
                foreach (var error in emailResult.Errors)
                {
                    result.AddError(error.Code, error.Description, AppServiceMessageType.Error);
                    return result;
                }
            }
            return result;
        }

        public AppServiceResult RequestPasswordReset(AppUser user)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult CreateClub()
        {
            throw new NotImplementedException();
        }

        public AppServiceResult JoinClub()
        {
            throw new NotImplementedException();
        }

        public AppServiceResult LeaveClub()
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<Club>> ListClubsByScore(int count)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<AppUser>> ListUsersByScore(int count)
        {
            throw new NotImplementedException();

            // var topOrgs = orgs.Select(m => new BountyViewModel()
            // {
            //     Bounty = m.Members.Select(n => n.BountyScore).Sum(),
            //     Name = m.Name
            // }).Where(m => m.Bounty > 0).OrderByDescending(m => m.Bounty).Take(10);

            // var topUsers = users.Select(m => new BountyViewModel()
            // {
            //     Bounty = m.BountyScore,
            //     Name = m.FirstName.Substring(0, 1) + ". " + m.LastName
            // }).Where(m => m.Bounty > 0).OrderByDescending(m => m.Bounty).Take(5);
        }

        public AppServiceResult UpdatePublicProfile(AppUser user, PublicProfile profile)
        {
            throw new NotImplementedException();

            // currentUser.FirstName = model.FirstName;
            // currentUser.Title = model.Title;
            // currentUser.LastName = model.LastName;
        }
    }
}