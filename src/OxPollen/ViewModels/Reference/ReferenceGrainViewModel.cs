using System.ComponentModel.DataAnnotations;

namespace OxPollen.ViewModels.Reference
{
    public class ReferenceGrainViewModel
    {
        [Required]
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }

        public int CollectionId { get; set; }

        [Required(ErrorMessage = "You must specify a scale for your image")]
        public double? ImagesScale { get; set; }
        [Required(ErrorMessage = "You must upload at least one file")]
        public string ImageOne { get; set; }
        public string ImageTwo { get; set; }
        public string ImageThree { get; set; }
        public string ImageFour { get; set; }
    }
}
