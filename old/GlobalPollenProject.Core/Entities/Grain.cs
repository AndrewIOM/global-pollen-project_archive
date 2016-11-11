using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.Core.Models
{
    public class Grain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual List<GrainImage> Images { get; set; }
        [Required]
        public virtual AppUser SubmittedBy { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public DateTime TimeAdded { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public double MaxSizeNanoMetres { get; set; }

        //Optional
        public int? AgeYearsBeforePresent { get; set; }
        public double? LockedBounty { get; set; }

        //Cached Properties (TODO remove)
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }

        //Identifications
        public virtual List<Identification> Identifications { get; set; }
        public Taxon IdentifiedAs { get; set; }

        public Grain()
        {
            IsDeleted = false;
        }
    }
}
