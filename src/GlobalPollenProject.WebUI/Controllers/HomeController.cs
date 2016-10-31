using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Index()
        {
            // var result = _grainService.GetUnidentifiedGrains(Taxonomy.Genus)
            //     .OrderByDescending(m => BountyUtility.Calculate(m)).Take(12).ToList();
            
            // var model = result.Select(m => new SimpleGrainViewModel()
            //     {
            //         Bounty = BountyUtility.Calculate(m),
            //         Id = m.Id,
            //         ImageLocation = m.Images != null ? m.Images.First().FileName : "",
            //         ThumbnailLocation = m.Images != null ? m.Images.First().FileNameThumbnail : "",
            //         TimeAdded = m.TimeAdded
            //     }).ToList();
            return View();
            //return View(model);
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
