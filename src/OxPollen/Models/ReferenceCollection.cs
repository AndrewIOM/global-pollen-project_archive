using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public class ReferenceCollection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Institution { get; set; }
        [Required]
        public string CountryCode { get; set; }

        public virtual List<ReferenceGrain> Grains { get; set; }
        public virtual AppUser User { get; set; }
    }
}
