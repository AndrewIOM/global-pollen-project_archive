using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OxPollen.Controllers
{
    public class ReferenceCollectionController : Controller
    {
        private readonly PollenDbContext _context;
        public ReferenceCollectionController(PollenDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(int? taxonId)
        {
            if (taxonId != null)
            {
                var taxon = _context.Taxa.FirstOrDefault(m => m.TaxonId == taxonId);
                return View("Details", taxon);
            }

            var model = _context.Taxa.Select(m => new TaxonViewModel()
            {
                CommonName = m.CommonName,
                Id = m.TaxonId,
                LatinName = m.LatinName
            }).ToList();
            return View(model);
        }
    }
}
