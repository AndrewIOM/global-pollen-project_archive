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
        public IActionResult Index(Taxonomy? rank)
        {
            var taxa = _context.Taxa.Include(m => m.Records).Include(m => m.ParentTaxa).ToList();
            if (rank.HasValue)
            {
                taxa = taxa.Where(m => m.Rank == rank.Value).ToList();
            }
            var model = taxa.Select(m => new TaxonViewModel()
            {
                Id = m.TaxonId,
                LatinName = m.LatinName,
                ConfirmedGrainsCount = m.Records.Count(),
                Rank = m.Rank
            }).ToList();
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return new BadRequestResult();

            var taxon = _context.Taxa.FirstOrDefault(m => m.TaxonId == id);
            if (taxon != null)
            {
                var grains = _context.PollenRecords.Include(m => m.Images).Include(m => m.Taxon)
                    .Where(m => m.Taxon.TaxonId == taxon.TaxonId).ToList();
                taxon.Records = grains;
                return View("View", taxon);
            }
            return new RedirectToActionResult("Index", "Taxon", null);
        }
    }
}
