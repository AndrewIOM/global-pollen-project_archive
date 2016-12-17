using Microsoft.AspNetCore.Mvc;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.WebUI.Extensions;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class TraitController : Controller
    {
        private readonly ITaxonomyService _service;

        public TraitController(ITaxonomyService service) {
            _service = service;
        }

        [HttpGet("size")]
        public IActionResult Size(int taxonId) {

            var sizes = _service.GetSizeProfile(taxonId);
            if (!sizes.IsValid)
            {
                ModelState.AddServiceErrors(sizes.Messages);
                return BadRequest(ModelState);
            }
            return Ok(sizes.Result);
        }

    }

}