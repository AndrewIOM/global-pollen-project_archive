using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.ViewModels;
using OxPollen.Utilities;
using OxPollen.Services.Abstract;
using OxPollen.Models;

namespace OxPollen.Controllers
{
    public class HomeController : Controller
    {
        public IGrainService _grainService;
        public HomeController(IGrainService grainService)
        {
            _grainService = grainService;
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Index()
        {
            var result = _grainService.GetUnidentifiedGrains(Taxonomy.Genus)
                .OrderByDescending(m => BountyUtility.Calculate(m)).Take(12).ToList();
            
            var model = result.Select(m => new SimpleGrainViewModel()
                {
                    Bounty = BountyUtility.Calculate(m),
                    Id = m.Id,
                    ImageLocation = m.Images.First().FileName,
                    ThumbnailLocation = m.Images.First().FileNameThumbnail,
                    TimeAdded = m.TimeAdded
                }).ToList();

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
