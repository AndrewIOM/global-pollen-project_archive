using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
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

            string cacheKey = "BackboneSuggest-" + q + rank + parent;
            List<BackboneTaxon> backboneResult;

            if (_memoryCache.TryGetValue(cacheKey, out backboneResult))
            {
                //Use cached copy of result
                backboneResult = _memoryCache.Get(cacheKey) as List<BackboneTaxon>;
            } else
            {
                //Retrieve new result and cache
                backboneResult = _taxonomyAppService.SearchBackbone(q, rank, parent).Result;
                _memoryCache.Set(cacheKey, backboneResult,
                    new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }

            return backboneResult;
        }

        [HttpGet("match")]
        public IActionResult Match(string family, string genus = null, string species = null)
        {
            if (string.IsNullOrEmpty(family)) return BadRequest();

            string cacheKey = "BackboneMatch-" + family + "-" + genus + "-" + species;
            BackboneTaxon backboneResult;

            if (_memoryCache.TryGetValue(cacheKey, out backboneResult))
            {
                //Use cached copy of result
                backboneResult = _memoryCache.Get(cacheKey) as BackboneTaxon;
            } else
            {
                //Retrieve new result and cache
                backboneResult = _taxonomyAppService.GetStatus(family, genus, species).Result;

                _memoryCache.Set(cacheKey, backboneResult,
                    new MemoryCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }

            return Ok(backboneResult);
        }

    }
}
