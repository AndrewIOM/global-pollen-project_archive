using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.ViewModels;
using OxPollen.Utilities;
using Microsoft.Data.Entity;
using OxPollen.Data.Concrete;

namespace OxPollen.Controllers
{
    public class HomeController : Controller
    {
        public OxPollenDbContext _context;
        public HomeController(OxPollenDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.UserGrains.Where(m => !m.IsDeleted).Include(m => m.Images).OrderByDescending(m => m.TimeAdded)
                .Take(10).ToList();
            var model = result.Select(m => new ReadOnlyGrainViewModel()
                {
                    Bounty = BountyUtility.Calculate(m.TimeAdded),
                    Id = m.Id,
                    ImageLocation = m.Images.First().FileName,
                    TimeAdded = m.TimeAdded
                }).Take(15).ToList();
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
