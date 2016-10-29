using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlobalPollenProject.Data.Models;

namespace GlobalPollenProject.WebUI.Models
{
    public class IdentificationViewModel : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Taxonomy
            if (string.IsNullOrEmpty(Family))
            {
                yield return new ValidationResult("Family is required", new[] { "Family" });
            }
            if (TaxonomicResolution == Taxonomy.Family)
            {
                if (!string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("You specified a Genus name for a Family ID. Check and resubmit.", new[] { "Genus" });
                }
                if (!string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("You specified a Species name for a Family ID. Check and resubmit.", new[] { "Species" });
                }
            }
            else if (TaxonomicResolution == Taxonomy.Genus)
            {
                if (string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("Genus is required for a Genus-rank ID.", new[] { "Genus" });
                }
                if (!string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("Species names are not valid when identifying to Genus level.", new[] { "Species" });
                }
            }
            else if (TaxonomicResolution == Taxonomy.Species)
            {
                if (string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("Genus is required for a Species-rank ID.", new[] { "Genus" });
                }
                if (string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("Species is required for a Species-rank ID.", new[] { "Species" });
                }
            }
        }
    }
}