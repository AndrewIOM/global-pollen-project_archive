using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.Data.Concrete;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using OxPollen.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNet.Hosting;
using OxPollen.Utilities;

namespace OxPollen.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly OxPollenDbContext _context;
        private readonly RoleManager<IdentityRole> _roleMan;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITaxonomyService _taxonService;
        private IHostingEnvironment _environment;
        public AdminController(ITaxonomyService taxon, OxPollenDbContext context, 
            RoleManager<IdentityRole> role, UserManager<AppUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _roleMan = role;
            _userManager = userManager;
            _taxonService = taxon;
            _environment = environment;
        }

        // GET: /Admin/
        public IActionResult Users()
        {
            var users = _context.Users.Include(m => m.Organisation).ToList();
            return View(users);
        }

        public IActionResult Taxa()
        {
            var taxa = _context.Taxa.OrderBy(m => m.LatinName).ToList();
            return View(taxa);
        }

        public IActionResult AddDigitiseRole(string userId)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user == null) return HttpBadRequest();

            var exists = _roleMan.RoleExistsAsync("Digitise").Result;
            if (!exists)
            {
                IdentityRole identityRole = new IdentityRole("Digitise");
                IdentityResult identityResult = _roleMan.CreateAsync(identityRole).Result;
            }
            var result = _userManager.AddToRoleAsync(user, "Digitise").Result;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult RemoveDigitiseRole(string userId)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user == null) return HttpBadRequest();

            var exists = _roleMan.RoleExistsAsync("Digitise").Result;
            if (!exists)
            {
                IdentityRole identityRole = new IdentityRole("Digitise");
                IdentityResult identityResult = _roleMan.CreateAsync(identityRole).Result;
            }
            var result = _userManager.RemoveFromRoleAsync(user, "Digitise").Result;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UserAdmin(string id, bool userIsAdmin)
        {
            if (id == User.GetUserId()) return HttpBadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();

            var exists = _roleMan.RoleExistsAsync("Admin").Result;
            if (!exists)
            {
                IdentityRole identityRole = new IdentityRole("Admin");
                IdentityResult identityResult = _roleMan.CreateAsync(identityRole).Result;
            }
            if (userIsAdmin)
            {
                var result = _userManager.AddToRoleAsync(user, "Admin").Result;
            }
            else
            {
                var result = _userManager.RemoveFromRoleAsync(user, "Admin").Result;
            }
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult BanUser(string id)
        {
            if (id == User.GetUserId()) return HttpBadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();
            var exists = _roleMan.RoleExistsAsync("Banned").Result;
            if (!exists)
            {
                IdentityRole identityRole = new IdentityRole("Banned");
                IdentityResult identityResult = _roleMan.CreateAsync(identityRole).Result;
            }
            var result = _userManager.AddToRoleAsync(user, "Banned").Result;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult VerifyEmail(string id)
        {
            if (id == User.GetUserId()) return HttpBadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();
            user.EmailConfirmed = true;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UnbanUser(string id)
        {
            if (id == User.GetUserId()) return HttpBadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return HttpBadRequest();
            var exists = _roleMan.RoleExistsAsync("Banned").Result;
            if (!exists)
            {
                IdentityRole identityRole = new IdentityRole("Banned");
                IdentityResult identityResult = _roleMan.CreateAsync(identityRole).Result;
            }
            var result = _userManager.RemoveFromRoleAsync(user, "Banned").Result;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlantList(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    await file.SaveAsAsync(Path.Combine(uploads, fileName));

                    //Seed plant list
                    var tool = new PlantListParser(fileName, _context);
                    tool.Refresh();
                }
            }
            return View();
        }

    }
}
