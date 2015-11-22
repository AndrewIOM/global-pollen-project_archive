using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.ViewModels
{
    public class AddGrainViewModel
    {
        [Required(ErrorMessage = "Use the map to enter a latitude")]
        public double? Latitude { get; set; }
        [Required(ErrorMessage = "Use the map to enter a longitude")]
        public double? Longitude { get; set; }
        public int? AgeYearsBeforePresent { get; set; }
        [Required(ErrorMessage = "You must specify a scale for your image")]
        public double? ImagesScale { get; set; }
    }
}
