using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.Data.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Im.Acm.Pollen.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using Im.Acm.Pollen.Utilities;

namespace Im.Acm.Pollen.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly PollenDbContext _context;
        private readonly RoleManager<IdentityRole> _roleMan;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITaxonomyService _taxonService;
        private IHostingEnvironment _environment;
        public AdminController(ITaxonomyService taxon, PollenDbContext context, 
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
            if (user == null) return BadRequest();

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
            if (user == null) return BadRequest();

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
            if (id == _userManager.GetUserId(User)) return BadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return BadRequest();

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
            if (id == _userManager.GetUserId(User)) return BadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return BadRequest();
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
            if (id == _userManager.GetUserId(User)) return BadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return BadRequest();
            user.EmailConfirmed = true;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UnbanUser(string id)
        {
            if (id == _userManager.GetUserId(User)) return BadRequest();

            var user = _context.Users.FirstOrDefault(m => m.Id == id);
            if (user == null) return BadRequest();
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
                   var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
                   await file.CopyToAsync(fileStream);

                   //Seed plant list
                   var tool = new PlantListParser(Path.Combine(uploads, fileName), _context);
                   tool.Refresh();
               }
           }
           return View();
        }

    }
}
