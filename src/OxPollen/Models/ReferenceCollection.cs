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
        [Display(Name = "Country")]
        public string CountryCode { get; set; }

        [Description("The global region for which the majority of taxa were collected. If the collection spans multiple regions, leave this blank.")]
        [Display(Name = "Regional Focus")]
        public string FocusRegion { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Website Address")]
        [DataType(DataType.Url)]
        public string WebAddress { get; set; }

        [Display(Name = "Email Address for Enquiries")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        public virtual List<ReferenceGrain> Grains { get; set; }

        public virtual AppUser User { get; set; }
    }
}
