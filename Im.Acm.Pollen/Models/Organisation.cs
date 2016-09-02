using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Im.Acm.Pollen.Models
{
    public class Organisation
    {
        [Key]
        public int OrganisationId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string CountryCode { get; set; }

        public virtual List<AppUser> Members { get; set; }
    }
}
