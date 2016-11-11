using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.Core.Models
{
    public class ReferenceGrain
    {
        [Key]
        public int ReferenceGrainId { get; set; }
        [Required]
        public Taxon Taxon { get; set; }

        [Required]
        public double MaxSizeNanoMetres { get; set; }
        public DateTime TimeAdded { get; set; }

        public virtual List<GrainImage> Images { get; set; }
        public virtual AppUser SubmittedBy { get; set; }
        public virtual ReferenceCollection Collection { get; set; }
    }
}
