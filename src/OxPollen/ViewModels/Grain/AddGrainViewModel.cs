﻿using System;
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
        [RegularExpression(@"^[1-9]+$", ErrorMessage = "Age must be numeric")]
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
