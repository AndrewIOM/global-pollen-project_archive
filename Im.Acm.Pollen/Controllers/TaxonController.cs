﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.ViewModels;
using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.ViewModels.Taxon;
using Microsoft.AspNetCore.Authorization;
using Im.Acm.Pollen.Data.Concrete;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace Im.Acm.Pollen.Controllers
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

            //Temporary Fix until issue fixed in .net core
            //Can't use multiple ThenIncludes in current build for single joined query...
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