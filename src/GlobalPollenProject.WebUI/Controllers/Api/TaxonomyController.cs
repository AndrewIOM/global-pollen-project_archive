using System.Linq;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers.Api
{
    [Route("api/[controller]")]
    public class TaxonomyController : Controller
    {
        private readonly ITaxonomyService _service;

        public TaxonomyController(ITaxonomyService service)
        {
            _service = service;
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