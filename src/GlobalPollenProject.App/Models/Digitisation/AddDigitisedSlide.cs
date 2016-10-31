using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.App.Models
{
    public class AddDigitisedSlide : IValidatableObject
    {
        [Required]
        public int? CollectionId { get; set; }

        [Required]
        public Taxonomy Rank { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }

        [Required]
        [Display(Name = "Maximum Grain Diameter")]
        public double? MaxGrainSize { get; set; }

        public List<string> Images { get; set; }
        public List<FocusImageViewModel> FocusImages { get; set; }

        public AddDigitisedSlide()
        {
            Images = new List<string>();
            FocusImages = new List<FocusImageViewModel>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Image Total
            var imageTotal = 0;
            if (Images != null)
            {
                imageTotal += Images.Count;
            }
            if (FocusImages != null)
            {
                imageTotal += FocusImages.Count;
            }
            if (imageTotal == 0)
            {
                yield return new ValidationResult("Your must upload at least one image", new[] { "Images" });
            }

            //Focus Image Format
            if (FocusImages != null)
            {
                foreach (var image in FocusImages)
                {
                    if (string.IsNullOrEmpty(image.FocusHighUrl) || string.IsNullOrEmpty(image.FocusLowUrl)
                        || string.IsNullOrEmpty(image.FocusMedHighUrl) || string.IsNullOrEmpty(image.FocusMedLowUrl)
                        || string.IsNullOrEmpty(image.FocusMedUrl))
                    {
                        yield return new ValidationResult("Focus Images must contain 5 focus levels", new[] { "FocusImages" });
                    }
                }
            }

            //Taxonomy
            if (string.IsNullOrEmpty(Family))
            {
                yield return new ValidationResult("Family is required", new[] { "Family" });
            }
            if (Rank == Taxonomy.Family)
            {
                if (!string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("You specified a Genus name for a Family. Check and resubmit.", new[] { "Genus" });
                }
                if (!string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("You specified a Species name for a Family. Check and resubmit.", new[] { "Species" });
                }
            }
            else if (Rank == Taxonomy.Genus)
            {
                if (string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("Genus is required for a Genus-rank taxon.", new[] { "Genus" });
                }
                if (!string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("You specified a Species name for a Family. Check and resubmit.", new[] { "Species" });
                }
            }
            else if (Rank == Taxonomy.Species)
            {
                if (string.IsNullOrEmpty(Genus))
                {
                    yield return new ValidationResult("Genus is required for a Species-rank taxon.", new[] { "Genus" });
                }
                if (string.IsNullOrEmpty(Species))
                {
                    yield return new ValidationResult("Species is required for a Species-rank taxon.", new[] { "Species" });
                }
            }
        }
    }

    public class FocusImageViewModel
    {
        public string FocusLowUrl { get; set; }
        public string FocusMedLowUrl { get; set; }
        public string FocusMedUrl { get; set; }
        public string FocusMedHighUrl { get; set; }
        public string FocusHighUrl { get; set; }
    }
}
