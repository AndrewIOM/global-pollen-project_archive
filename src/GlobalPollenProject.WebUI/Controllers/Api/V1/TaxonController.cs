using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class TaxonController : Controller
    {
        private readonly ITaxonomyService _taxonomyAppService;
        private readonly IMemoryCache _memoryCache;

        public TaxonController() {
            
        }

        public IActionResult Get() {
            return Ok("Cool");
        }



        // public IActionResult Match(string latinName, Rank rank) {
        //     var result = _service.GetAll().FirstOrDefault(m => m.LatinName == latinName && m.Rank == rank);
        //     var model = new TaxonDTO() {
        //         TaxonId = result.TaxonId
        //     };
        //     return Ok(model);
        // }
    }
}