using Microsoft.AspNetCore.Mvc;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using System.Linq;
using GlobalPollenProject.WebUI.Models.Taxon;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonService;
        public TaxonController(ITaxonomyService taxonService)
        {
            _taxonService = taxonService;
        }

        public IActionResult Index(Rank? rank, int p = 1, int s = 40, string query = null)
        {
            var model = _taxonService.ListGPPTaxa(s, p).Result;
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return BadRequest();
            var taxon = _taxonService.GetTaxon(id);
            if (taxon == null) return NotFound();
            return View("View", taxon);
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
