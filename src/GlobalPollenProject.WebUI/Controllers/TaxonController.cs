using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.ViewModels;
using GlobalPollenProject.WebUI.Services.Abstract;
using GlobalPollenProject.WebUI.ViewModels.Taxon;
using Microsoft.AspNetCore.Authorization;
using GlobalPollenProject.WebUI.Data.Concrete;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonService;
        private readonly PollenDbContext _context;
        public TaxonController(ITaxonomyService taxonService, PollenDbContext context)
        {
            _taxonService = taxonService;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(Taxonomy? rank, int p = 1, string query = null)
        {
            var rankFilter = rank.HasValue ? rank.Value : Taxonomy.Genus;

            // Complex context-based includes currently required to overcome issues
            // present in initial release of Entity Framework Core (1.0.0)
            var allTaxa = _context.Taxa
                .Include(m => m.ChildTaxa)
                .Include(m => m.UserGrains)
                .ThenInclude(n => n.Images)
                .Include(m => m.ReferenceGrains)
                .ThenInclude(n => n.Images)
                .OrderBy(m => m.LatinName)
                .ToList()
                .Where(m => m.Rank == rankFilter).ToList();

            if (!string.IsNullOrEmpty(query)) { allTaxa = allTaxa.Where(m => m.LatinName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList(); }
            var page = allTaxa.Skip((p - 1) * 40).Take(40).ToList();

            var model = new TaxonIndexViewModel();
            model.NumberOfPages = (int)Math.Ceiling(allTaxa.Count() / 40.0);
            model.CurrentPage = p;
            model.PageSize = 40;
            model.Query = query;
            model.Rank = rankFilter;

            foreach (var taxon in page)
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
                model.Taxa.Add(viewModel);
            }
            return View(model);
        }

        public IActionResult View(int id)
        {
            if (id == 0) return BadRequest();
            var taxon = _taxonService.GetById(id);
            if (taxon == null) return NotFound();

            List<Grain> userGrains = taxon.UserGrains;
            List<ReferenceGrain> refGrains = taxon.ReferenceGrains;
            foreach (var child in taxon.ChildTaxa)
            {
                var childTaxon = _taxonService.GetById(child.TaxonId);
                userGrains.AddRange(childTaxon.UserGrains);
                refGrains.AddRange(childTaxon.ReferenceGrains);

                foreach (var subChild in childTaxon.ChildTaxa)
                {
                    var subChildTaxa = _taxonService.GetById(subChild.TaxonId);
                    userGrains.AddRange(subChildTaxa.UserGrains);
                    refGrains.AddRange(subChildTaxa.ReferenceGrains);
                }
            }

            var model = new TaxonDetailViewModel()
            {
                GbifId = taxon.GbifId,
                Id = taxon.TaxonId,
                LatinName = taxon.LatinName,
                NeotomaId = taxon.NeotomaId,
                Rank = taxon.Rank,
                ReferenceGrains = refGrains,
                SubmittedGrains = userGrains,
                ParentTaxon = taxon.ParentTaxa,
                SubTaxa = taxon.ChildTaxa
            };
            return View("View", model);
        }

        public IActionResult NeotomaLookup(int neotomaId)
        {
            if (neotomaId == 0) return BadRequest();
            var match = _context.Taxa.FirstOrDefault(m => m.NeotomaId == neotomaId);
            if (match == null) return NotFound();
            return RedirectToAction("View", new { id = match.TaxonId });
        }

        public IActionResult GbifLookup(int gbifId)
        {
            if (gbifId == 0) return BadRequest();
            var match = _context.Taxa.FirstOrDefault(m => m.GbifId == gbifId);
            if (match == null) return NotFound();
            return RedirectToAction("View", new { id = match.TaxonId });
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
        public IActionResult RefreshConnections()
        {
            var taxa = _taxonService.GetAll().ToList();
            foreach (var taxon in taxa)
            {
                _taxonService.RefreshConnections(taxon.TaxonId);
            }

            return Ok();
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int id)
        {
            var taxon = _taxonService.GetById(id);
            if (taxon == null) return BadRequest();

            var refCount = taxon.ReferenceGrains.Count();
            var grainCount = taxon.UserGrains.Count();
            if (grainCount == 0 && refCount == 0)
            {
                _taxonService.RemoveTaxon(taxon.TaxonId);
                return RedirectToAction("Taxa", "Admin");
            }
            return BadRequest();
        }

        public IActionResult Suggest(string searchTerm)
        {
            var result = _taxonService.Suggest(searchTerm).Take(10).Select(m => new TaxonSuggest()
            {
                Id = m.TaxonId,
                Name = m.LatinName,
                Rank = m.Rank
            });
            return Ok(result);
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
