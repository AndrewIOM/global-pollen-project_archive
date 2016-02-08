using OxPollen.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace OxPollen.ViewModels.Reference
{
    public class ReferenceGrainViewModel : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Images == null)
            {
                yield return new ValidationResult("Your must upload at least one image", new[] { "Images" });
            }
            else
            {
                if (Images.Count == 0)
                {
                    yield return new ValidationResult("Your must upload at least one image", new[] { "Images" });
                }
            }

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
}
