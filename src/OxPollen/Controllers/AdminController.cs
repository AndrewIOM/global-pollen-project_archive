using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.Data.Concrete;

namespace OxPollen.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly OxPollenDbContext _context;
        public AdminController(OxPollenDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult BanUser(string id)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();
            user.LockoutEnabled = true;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UnbanUser(string id)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();
            user.LockoutEnabled = false;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult Pollen()
        {
            var pollen = _context.Taxa.ToList();
            return View("PollenView", pollen);
        }

        public IActionResult AddPollen()
        {
            return View("PollenAdd", new Taxon());
        }
    }
}
