using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using Microsoft.Data.Entity;
using OxPollen.Data.Concrete;
using OxPollen.ViewModels.Taxon;

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
            var taxa = _context.Taxa.Include(m => m.SubmittedGrains).ToList();
            var rankFilter = rank.HasValue ? rank.Value : Taxonomy.Genus;
            var model = taxa.Where(m => m.Rank == rankFilter).Select(m => new TaxonViewModel()
            {
                Id = m.TaxonId,
                LatinName = m.LatinName,
                ConfirmedGrainsCount = m.SubmittedGrains.Count(),
                Rank = m.Rank
            }).OrderBy(m => m.LatinName).ToList();
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return new BadRequestResult();

            var taxon = _context.Taxa.Include(m => m.ReferenceGrains).FirstOrDefault(m => m.TaxonId == id);
            if (taxon != null)
            {
                List<Grain> grains = null;
                List<ReferenceGrain> refGrains = null;
                if (taxon.Rank == Taxonomy.Species) grains = _context.UserGrains.Include(m => m.Images).Where(m => m.Species == taxon.LatinName).ToList();
                if (taxon.Rank == Taxonomy.Genus) grains = _context.UserGrains.Include(m => m.Images).Where(m => m.Genus == taxon.LatinName).ToList();
                if (taxon.Rank == Taxonomy.Family) grains = _context.UserGrains.Include(m => m.Images).Where(m => m.Family == taxon.LatinName).ToList();
                taxon.SubmittedGrains = grains;

                if (taxon.Rank == Taxonomy.Species) refGrains = _context.ReferenceGrains.Include(m => m.Images).Where(m => m.Species == taxon.LatinName).ToList();
                if (taxon.Rank == Taxonomy.Genus) refGrains = _context.ReferenceGrains.Include(m => m.Images).Where(m => m.Genus == taxon.LatinName).ToList();
                if (taxon.Rank == Taxonomy.Family) refGrains = _context.ReferenceGrains.Include(m => m.Images).Where(m => m.Family == taxon.LatinName).ToList();
                taxon.ReferenceGrains = refGrains;
                return View("View", taxon);
            }
            return new RedirectToActionResult("Index", "Taxon", null);
        }
    }
}
