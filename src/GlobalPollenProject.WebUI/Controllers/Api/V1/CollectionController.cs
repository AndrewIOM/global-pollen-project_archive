using System;
using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class CollectionController : Controller
    {
        private readonly IDigitisationService _service;
        public CollectionController(IDigitisationService service) {
            _service = service;
        }

        public IActionResult List() {
            throw new NotImplementedException();
        }

    }
}