using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public class Identification
    {
        public int IdentificationId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeIdentified { get; set; }
        public string TaxonName { get; set; }

        //Navigation Properties
        public PollenRecord Record { get; set; }
    }
}
