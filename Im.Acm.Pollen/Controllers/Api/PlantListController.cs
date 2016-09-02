using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Im.Acm.Pollen.Data.Concrete;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Im.Acm.Pollen.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlantListController : Controller
    {
        private readonly PollenDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public PlantListController(PollenDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet("suggest")]
        public IEnumerable<BackboneTaxon> Suggest(string q, Taxonomy? rank, string parent = null)
        {
            if (string.IsNullOrEmpty(q)) return null;

            string cacheKey = "PlantListSuggest-" + q + rank + parent;
            List<BackboneTaxon> backboneResult;

            if (_memoryCache.TryGetValue(cacheKey, out backboneResult))
            {
                //Use cached copy of result
                backboneResult = _memoryCache.Get(cacheKey) as List<BackboneTaxon>;
            } else
            {
                //Get from database and convert result to DTO
                IQueryable<PlantListTaxon> result = _context.PlantListTaxa.Include(m => m.ParentTaxa);
                if (!string.IsNullOrEmpty(parent)) { result = result.Where(m => m.ParentTaxa.LatinName.Equals(parent, StringComparison.OrdinalIgnoreCase)); }
                if (rank.HasValue) { result = result.Where(m => m.Rank == rank); }
                result = result.Where(m => m.LatinName.StartsWith(q));

                var list = result.OrderBy(m => m.LatinName).Take(10).ToList();
                backboneResult = list.Select(m => new BackboneTaxon()
                {
                    Id = m.Id,
                    LatinName = m.LatinName,
                    Rank = m.Rank,
                    Status = m.Status,
                    ParentLatinName = m.Rank == Taxonomy.Family ? "" : m.ParentTaxa.LatinName
                }).ToList();

                //Cache result
                _memoryCache.Set(cacheKey, backboneResult,
                    new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }

            return backboneResult;
        }

    }
}
