using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("The country where the collection is located")]
        [DisplayName("Country")]
        public string CountryCode { get; set; }
        [Required]
        [Description("The global region for which the majority of taxa were collected")]
        [DisplayName("Regional Focus")]
        public string FocusRegion { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual List<ReferenceGrain> Grains { get; set; }
        public virtual AppUser User { get; set; }
    }
}
