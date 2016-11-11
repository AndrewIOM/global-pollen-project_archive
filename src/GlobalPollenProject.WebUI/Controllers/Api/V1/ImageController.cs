using System;
using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers.Api.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class ImageController : Controller
    {
        private readonly ITaxonomyService _service;

        public ImageController(ITaxonomyService service) {
            _service = service;
        }

        public IActionResult Get(int taxonId, bool recursive) {
            throw new NotImplementedException();
            // var taxon = _service.GetGPPTaxon(taxonId);
            // if (taxon == null) return BadRequest();
            
            // var result = new List<ImageResult>();
            // foreach (var slide in taxon.ReferenceGrains) {
            //     foreach (var image in slide.Images) {
            //         result.Add(new ImageResult() {
            //             Stack = image.ImageUrls,
            //             Origin = SlideOrigin.DigitisedFromReferenceCollection,
            //             CollectionId = slide.Collection.Id,
            //             Citation = slide.Collection.Name
            //         });
            //     }
            // }

            // Complete recursion if requested
            // ....

            // return Ok(result);
        }
    }
}