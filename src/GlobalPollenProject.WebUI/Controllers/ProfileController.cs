using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> View(string username)
        {
            var profileResult = await _userService.GetPublicProfile(username);
            if (!profileResult.IsValid) return BadRequest();
            return View(profileResult.Result);
        }

    }
}
