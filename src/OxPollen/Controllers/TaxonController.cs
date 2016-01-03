using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Taxon;

namespace OxPollen.Controllers
{
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonService;
        public TaxonController(ITaxonomyService taxonService)
        {
            _taxonService = taxonService;
        }

        // GET: /<controller>/
        public IActionResult Index(Taxonomy? rank)
        {
            var rankFilter = rank.HasValue ? rank.Value : Taxonomy.Genus;
            var taxa = _taxonService.GetAll().Where(m => m.Rank == rankFilter).ToList();
            var model = taxa.Select(m => new TaxonViewModel()
            {
                Id = m.TaxonId,
                LatinName = m.LatinName,
                ReferenceGrainsCount = _taxonService.GetReferenceGrains(m).Count(),
                UserSubmissionsConfirmedCount = _taxonService.GetUserGrains(m).Count(),
                Rank = m.Rank
            }).OrderBy(m => m.LatinName).ToList();
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return HttpBadRequest();
            var taxon = _taxonService.GetById(id);
            if (taxon == null) return HttpNotFound();

            var model = new TaxonDetailViewModel()
            {
                GbifId = taxon.GbifId,
                Id = taxon.TaxonId,
                LatinName = taxon.LatinName,
                NeotomaId = taxon.NeotomaId,
                Rank = taxon.Rank,
                ReferenceGrains = _taxonService.GetReferenceGrains(taxon).ToList(),
                SubmittedGrains = _taxonService.GetUserGrains(taxon).ToList()
            };
            return View("View", model);
        }
    }
}
