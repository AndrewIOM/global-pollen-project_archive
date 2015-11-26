using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OxPollen.ViewModels
{
    public class IdentificationViewModel
    {
        public int GrainId { get; set; }
        public DateTime TimeAdded { get; set; }
        public string IdentifiedFamily { get; set; }
        public string IdentifiedGenus { get; set; }
        public string IdentifiedSpecies { get; set; }
        public List<string> ImageUrls { get; set; }
        public double ImageScale { get; set; }
        public double? Age { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Identification> Identifications { get; set; }

        //Identification Form
        public bool AlreadyIdentifiedByUser { get; set; }
        public Identification UserIdentification { get; set; }

        [Required]
        public Taxonomy TaxonomicResolution { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The name must be alphabetic only.")]
        public string Family { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The name must be alphabetic only.")]
        public string Genus { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "The name must be alphabetic only.")]
        public string Species { get; set; }
    }
}