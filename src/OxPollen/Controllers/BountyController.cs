using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Runtime;
using OxPollen.Models;
using OxPollen.Services;
using OxPollen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Controllers
{
    public class BountyController : Controller
    {
        private readonly PollenDbContext _context;
        private readonly IdentificationService _idService;
        private IApplicationEnvironment _hostingEnvironment;

        public BountyController(PollenDbContext context, IdentificationService idService, IApplicationEnvironment env)
        {
            _context = context;
            _idService = idService;
            _hostingEnvironment = env;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = _context.Users.Select(m => new BountyViewModel()
            {
                FullName = m.FullName,
                Organisation = m.Organisation, 
                Bounty = m.Bounty
            }).ToList();
            return View(model);
        }

    }
}
