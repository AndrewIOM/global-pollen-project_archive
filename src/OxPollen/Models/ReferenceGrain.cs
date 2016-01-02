using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public class ReferenceGrain
    {
        [Key]
        public int ReferenceGrainId { get; set; }

        public virtual List<GrainImage> Images { get; set; }
        public virtual AppUser SubmittedBy { get; set; }
        public virtual ReferenceCollection Collection { get; set; }
        public DateTime TimeAdded { get; set; }

        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
    }
}
