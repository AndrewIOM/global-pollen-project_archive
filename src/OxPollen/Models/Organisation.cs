using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
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
