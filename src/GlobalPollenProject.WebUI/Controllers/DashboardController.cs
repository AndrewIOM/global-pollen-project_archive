using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    [Authorize]
    [ApiVersionNeutral]
    public class DashboardController : Controller
    {
        private readonly IIdentificationService _idAppService;

        public DashboardController(IIdentificationService idService)
        {
            _idAppService = idService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UnknownGrains(int pageSize = 40, int page = 1)
        {
            var appResult = await _idAppService.GetMyUnknownGrains(pageSize, page);
            if (!appResult.IsValid) return BadRequest();
            return View(appResult.Result);
        }

    }
}