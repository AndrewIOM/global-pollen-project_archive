using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Models.Api.V1.Image
{
    public class ImageResult
    {
        public List<string> Stack { get; set; }
        public SlideOrigin Origin { get; set; }
        public int CollectionId { get; set; }
        public string Citation { get; set; }
    }

    public enum SlideOrigin
    {
        DigitisedFromReferenceCollection = 1,
        UploadedAsUnknown = 2
    }

}