using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class TraitController : Controller
    {
        // private readonly ITraitService _service;

        // public TraitController(ITraitService service) {
        //     _service = service;
        // }

        // public IActionResult Size(int taxonId) {

        //     var sizes = _service.ListSizes(taxonId);
        //     if (sizes == null) return BadRequest();
        //     return Ok(sizes);
        // }

    }

}