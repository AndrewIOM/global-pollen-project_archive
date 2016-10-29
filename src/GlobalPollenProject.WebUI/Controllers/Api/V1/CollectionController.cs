using System;
using GlobalPollenProject.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class CollectionController : Controller
    {
        private readonly IReferenceService _service;
        public CollectionController(IReferenceService service) {
            _service = service;
        }

        public IActionResult List() {
            throw new NotImplementedException();
        }

    }
}