using System.Linq;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _service;
        private readonly ITaxonomyBackbone _backbone;
        private readonly IMemoryCache _memoryCache;

        public TaxonController() {

        }

        public IActionResult Get() {
            return Ok("Cool");
        }



        public IActionResult Match(string latinName, Taxonomy rank) {
            var result = _service.GetAll().FirstOrDefault(m => m.LatinName == latinName && m.Rank == rank);
            var model = new TaxonDTO() {
                TaxonId = result.TaxonId
            };
            return Ok(model);
        }

        private class TaxonDTO 
        {
            public int TaxonId { get; set; }
            public TaxonomicStatus Status { get; set; }
            public int DigitisedSlidesCount { get; set; }
            public int UserSubmissionsCount { get; set; }
            public string KeyImageUrl { get; set; }
        }
    }
}