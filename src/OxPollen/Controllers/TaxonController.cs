using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using Microsoft.Data.Entity;

namespace OxPollen.Controllers
{
    public class TaxonController : Controller
    {
        private readonly OxPollenDbContext _context;
        public TaxonController(OxPollenDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var taxa = _context.Taxa.Include(m => m.Records).Include(m => m.ParentTaxa).ToList();
            var model = taxa.Select(m => new TaxonViewModel()
            {
                Id = m.TaxonId,

                LatinName = m.Rank == Taxonomy.Species ? m.ParentTaxa.LatinName + " " + m.LatinName : m.LatinName,
                KeyImageUrl = "/images/pollensample.jpg",
                ConfirmedGrainsCount = m.Records.Count(),
                Rank = m.Rank
            }).ToList();
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return new BadRequestResult();
            var taxa = _context.Taxa.Include(m => m.Records).ToList();

            var taxon = taxa.FirstOrDefault(m => m.TaxonId == id);
            if (taxon != null)
            {
                return View("View", taxon);
            }
            return new RedirectToActionResult("Index", "ReferenceCollection", null);
        }
    }
}
