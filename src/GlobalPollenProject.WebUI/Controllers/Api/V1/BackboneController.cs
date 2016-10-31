using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Interfaces;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [Route("api/[controller]")]
    public class BackboneController : Controller
    {
        private readonly ITaxonomyService _taxonomyAppService;
        private readonly IMemoryCache _memoryCache;
        public BackboneController(ITaxonomyService taxonomyAppService, IMemoryCache memoryCache)
        {
            _taxonomyAppService = taxonomyAppService;
            _memoryCache = memoryCache;
        }

        [HttpGet("suggest")]
        public IEnumerable<BackboneTaxon> Suggest(string q, Rank? rank, string parent = null)
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
                //Retrieve new result and cache
                backboneResult = _taxonomyAppService.Search(q, rank, parent);
                _memoryCache.Set(cacheKey, backboneResult,
                    new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }

            return backboneResult;
        }

    }
}
