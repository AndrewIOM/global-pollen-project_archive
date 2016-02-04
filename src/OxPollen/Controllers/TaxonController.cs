using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Taxon;
using Microsoft.AspNet.Authorization;

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
                Rank = m.Rank,
                ImageFilename = _taxonService.GetRandomImageForTaxon(m.TaxonId),
                Children = m.ChildTaxa.Select(n => new TaxonViewModel()
                {
                    Children = null,
                    Id = n.TaxonId,
                    LatinName = n.LatinName,
                    Rank = n.Rank,
                    ReferenceGrainsCount = _taxonService.GetReferenceGrains(n).Count(),
                    UserSubmissionsConfirmedCount = _taxonService.GetUserGrains(n).Count()
                }).ToList()
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
                SubmittedGrains = _taxonService.GetUserGrains(taxon).ToList(),
                ParentTaxon = taxon.ParentTaxa,
                SubTaxa = taxon.ChildTaxa
            };
            return View("View", model);
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Purge()
        {
            var taxa = _taxonService.GetAll().ToList();
            foreach (var taxon in taxa)
            {
                if (taxon.ChildTaxa.Count == 0)
                {
                    var refCount = _taxonService.GetReferenceGrains(taxon).ToList().Count();
                    var grainCount = _taxonService.GetUserGrains(taxon).ToList().Count();
                    if (grainCount == 0 && refCount == 0)
                    {
                        _taxonService.RemoveTaxon(taxon.TaxonId);
                    }
                }
            }

            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int id)
        {
            var taxon = _taxonService.GetById(id);
            if (taxon == null) return HttpBadRequest();

            var refCount = _taxonService.GetReferenceGrains(taxon).Count();
            var grainCount = _taxonService.GetUserGrains(taxon).Count();
            if (grainCount == 0 && refCount == 0)
            {
                _taxonService.RemoveTaxon(taxon.TaxonId);
                return RedirectToAction("Taxa", "Admin");
            }
            return HttpBadRequest();
        }
    }
}
