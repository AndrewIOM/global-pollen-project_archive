using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class HomeController : Controller
    {
        private IIdentificationService _idService;

        public HomeController(IIdentificationService idService)
        {
            _idService = idService;
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Index()
        {
            // TODO sort by most wanted (requires paged App Services)
            var searchCriteria = new GrainSearchFilter();
            var model = _idService.GetUnknownGrains(searchCriteria, 12, 1).Result;
            return View(model);
        }

        public IActionResult Guide()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
