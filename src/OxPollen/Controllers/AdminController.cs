using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using OxPollen.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OxPollen.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly PollenDbContext _context;
        public AdminController(PollenDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
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
