using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models
{
    public class IdentificationFormViewModel : IValidatableObject
    {
        public UnknownGrain Grain { get; set; }

        //Identification Form
        public bool AlreadyIdentifiedByUser { get; set; }
        public IdentificationDto UserIdentification { get; set; }

        [Required]
        public Rank TaxonomicResolution { get; set; }
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
            if (TaxonomicResolution == Rank.Family)
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
            else if (TaxonomicResolution == Rank.Genus)
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
            else if (TaxonomicResolution == Rank.Species)
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