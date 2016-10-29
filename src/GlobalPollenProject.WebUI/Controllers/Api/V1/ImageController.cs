using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.WebUI.Models.Api.V1.Image;
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
            var taxon = _service.GetById(taxonId);
            if (taxon == null) return BadRequest();
            
            var result = new List<ImageResult>();
            foreach (var slide in taxon.ReferenceGrains) {
                foreach (var image in slide.Images) {
                    var images = new List<string>() { image.FocusLowUrl, image.FocusMedLowUrl, image.FocusMedUrl, image.FocusMedHighUrl, image.FocusHighUrl };
                    images = images.Where(m => !string.IsNullOrEmpty(m)).ToList();

                    result.Add(new ImageResult() {
                        Stack = images,
                        Origin = SlideOrigin.DigitisedFromReferenceCollection,
                        CollectionId = slide.Collection.Id,
                        Citation = slide.Collection.Name
                    });
                }
            }

            // Complete recursion if requested
            // ....

            return Ok(result);
        }
    }
}