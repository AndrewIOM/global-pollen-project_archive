using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Taxon;
using Microsoft.AspNet.Authorization;
using OxPollen.Data.Concrete;
using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace OxPollen.Controllers
{
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonService;
        private readonly OxPollenDbContext _context;
        public TaxonController(ITaxonomyService taxonService, OxPollenDbContext context)
        {
            _taxonService = taxonService;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(Taxonomy? rank)
        {
            var rankFilter = rank.HasValue ? rank.Value : Taxonomy.Genus;

            //Temporary Fix until EF7 RC2 is released. 
            //Can't use multiple ThenIncludes in current build for single joined query...
            var taxa = _context.Taxa
                .Include(m => m.ChildTaxa)
                .Include(m => m.UserGrains)
                .ThenInclude(n => n.Images)
                .Include(m => m.ReferenceGrains)
                .ThenInclude(n => n.Images)
                .OrderBy(m => m.LatinName)
                .ToList();
            var ofRank = taxa.Where(m => m.Rank == rankFilter);

            var model = new List<TaxonViewModel>();
            foreach (var taxon in ofRank)
            {
                var viewModel = new TaxonViewModel()
                {
                    Id = taxon.TaxonId,
                    LatinName = taxon.LatinName,
                    Rank = taxon.Rank,
                    ReferenceGrainsCount = taxon.ReferenceGrains.Count,
                    UserSubmissionsConfirmedCount = taxon.UserGrains.Count,
                    ImageFilename = GetImageRecursive(taxon),
                    Children = taxon.ChildTaxa.Select(t => new TaxonChildViewModel()
                    {
                        Id = t.TaxonId,
                        LatinName = t.LatinName
                    }).ToList()
                };

                if (taxon.ChildTaxa != null)
                {
                    foreach (var child in taxon.ChildTaxa)
                    {
                        viewModel.ReferenceGrainsCount += child.ReferenceGrains.Count;
                        viewModel.UserSubmissionsConfirmedCount += child.UserGrains.Count;

                        if (child.ChildTaxa != null)
                        {
                            foreach (var subChild in child.ChildTaxa)
                            {
                                viewModel.ReferenceGrainsCount += subChild.ReferenceGrains.Count;
                                viewModel.UserSubmissionsConfirmedCount += subChild.UserGrains.Count;
                            }
                        }

                    }
                }

                model.Add(viewModel);
            }
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
                ReferenceGrains = taxon.ReferenceGrains,
                SubmittedGrains = taxon.UserGrains,
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
                    var refCount = taxon.ReferenceGrains.Count();
                    var grainCount = taxon.UserGrains.Count();
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

            var refCount = taxon.ReferenceGrains.Count();
            var grainCount = taxon.UserGrains.Count();
            if (grainCount == 0 && refCount == 0)
            {
                _taxonService.RemoveTaxon(taxon.TaxonId);
                return RedirectToAction("Taxa", "Admin");
            }
            return HttpBadRequest();
        }

        private string GetImageRecursive(Taxon taxon)
        {
            if (taxon.ReferenceGrains.Count > 0)
            {
                if (taxon.ReferenceGrains.First().Images.Count > 0)
                {
                    return taxon.ReferenceGrains.First().Images.First().FileNameThumbnail;
                }
            }
            else if (taxon.UserGrains.Count > 0)
            {
                if (taxon.UserGrains.First().Images.Count > 0)
                {
                    return taxon.UserGrains.First().Images.First().FileNameThumbnail;
                }
            }
            foreach (var child in taxon.ChildTaxa)
            {
                var result = GetImageRecursive(child);
                if (!string.IsNullOrEmpty(result)) return result;
            }
            return null;
        }

    }
}
