using Microsoft.AspNetCore.Mvc;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using System.Linq;
using GlobalPollenProject.WebUI.Models.Taxon;
using GlobalPollenProject.WebUI.Extensions;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonService;
        private readonly IIdentificationService _idService;
        private readonly IDigitisationService _digitiseService;
        public TaxonController(ITaxonomyService taxonService, IIdentificationService idService, IDigitisationService digitiseService)
        {
            _taxonService = taxonService;
            _idService = idService;
            _digitiseService = digitiseService;
        }

        public IActionResult Index(Rank? rank, int p = 1, int s = 40, string query = null)
        {
            Rank r = rank.HasValue ? rank.Value : Rank.Genus;
            var filter = new TaxonFilter();
            filter.Rank = r;
            filter.LatinName = query;

            var serviceResult = _taxonService.ListGPPTaxa(filter, s, p);
            var model = new TaxonIndexViewModel()
            {
                CurrentPage = serviceResult.CurrentPage,
                NumberOfPages = serviceResult.PageCount,
                PageSize = serviceResult.PageSize,
                Query = query,
                Rank = r,
                Taxa = serviceResult.Result.ToList()
            };
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return BadRequest();
            var taxon = _taxonService.GetTaxon(id);
            if (!taxon.IsValid) return NotFound();
            return View("View", taxon.Result);
        }

        public IActionResult ListReferenceMaterial(int p, int pageSize, int taxonId)
        {
            var criteria = new SlideSearchCriteria()
            {
                TaxonId = taxonId
            };
            var appResult = _digitiseService.GetSlides(pageSize, p, criteria);
            if (!appResult.IsValid)
            {
                ModelState.AddServiceErrors(appResult.Messages);
                return BadRequest(ModelState);
            }
            return Ok(appResult.Result);
        }

        public IActionResult ListUserSubmissions(int p, int pageSize, int taxonId)
        {
            var criteria = new GrainSearchFilter();
            criteria.TaxonId = taxonId;
            var appResult = _idService.GetUnknownGrains(criteria, pageSize, p);
            if (!appResult.IsValid)
            {
                ModelState.AddServiceErrors(appResult.Messages);
                return BadRequest(ModelState);
            }
            return Ok(appResult.Result);
        }

        public IActionResult Suggest(string searchTerm, int p = 1, int s = 40)
        {
            var result = _taxonService.GetTaxa(searchTerm, s, p, null).Result.Select(m => new TaxonSuggest()
            {
                Id = m.Id,
                Name = m.LatinName,
                Rank = m.Rank
            });
            return Ok(result);
        }

        // [Authorize(Policy = "AdminOnly")]
        // public IActionResult Purge()
        // {
        //     var taxa = _taxonService.GetAll().ToList();
        //     foreach (var taxon in taxa)
        //     {
        //         if (taxon.ChildTaxa.Count == 0)
        //         {
        //             var refCount = taxon.ReferenceGrains.Count();
        //             var grainCount = taxon.UserGrains.Count();
        //             if (grainCount == 0 && refCount == 0)
        //             {
        //                 _taxonService.RemoveTaxon(taxon.TaxonId);
        //             }
        //         }
        //     }

        //     return Ok();
        // }

        // [Authorize(Policy = "AdminOnly")]
        // public IActionResult RefreshConnections()
        // {
        //     var taxa = _taxonService.GetAll().ToList();
        //     foreach (var taxon in taxa)
        //     {
        //         _taxonService.RefreshConnections(taxon.TaxonId);
        //     }

        //     return Ok();
        // }

        // [Authorize(Policy = "AdminOnly")]
        // public IActionResult Delete(int id)
        // {
        //     var taxon = _taxonService.GetById(id);
        //     if (taxon == null) return BadRequest();

        //     var refCount = taxon.ReferenceGrains.Count();
        //     var grainCount = taxon.UserGrains.Count();
        //     if (grainCount == 0 && refCount == 0)
        //     {
        //         _taxonService.RemoveTaxon(taxon.TaxonId);
        //         return RedirectToAction("Taxa", "Admin");
        //     }
        //     return BadRequest();
        // }

    }
}
