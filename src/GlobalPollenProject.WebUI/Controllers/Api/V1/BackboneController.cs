using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Models;
using GlobalPollenProject.Data.Models.Enums;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [Route("api/[controller]")]
    public class BackboneController : Controller
    {
        private readonly ITaxonomyBackbone _backbone;
        private readonly IMemoryCache _memoryCache;
        public BackboneController(ITaxonomyBackbone backbone, IMemoryCache memoryCache)
        {
            _backbone = backbone;
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
                //Retrieve new result and cache
                backboneResult = _backbone.Suggest(q, rank, parent);
                _memoryCache.Set(cacheKey, backboneResult,
                    new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }

            return backboneResult;
        }

    }
}
