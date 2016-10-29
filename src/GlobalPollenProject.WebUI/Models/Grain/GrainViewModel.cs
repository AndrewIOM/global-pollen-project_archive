using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.WebUI.Models
{
    public class GrainViewModel
    {
        [Required(ErrorMessage = "Use the map to enter a latitude")]
        public double? Latitude { get; set; }
        [Required(ErrorMessage = "Use the map to enter a longitude")]
        public double? Longitude { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Age must be numeric")]
        public int? AgeYearsBeforePresent { get; set; }
        [Required(ErrorMessage = "You must specify a scale for your image")]
        public double? ImagesScale { get; set; }
        [Required(ErrorMessage = "You must upload at least one file")]
        public string ImageOne { get; set; }
        public string ImageTwo { get; set; }
        public string ImageThree { get; set; }
        public string ImageFour { get; set; }
    }
}
