using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Models
{
    public class Identification
    {
        public int IdentificationId { get; set; }
        public PollenRecord Record { get; set; }
        public string UserId { get; set; }
        public DateTime DateOfIdentification { get; set; }

        public string TaxonName { get; set; }
        public int GbifId { get; set; }
    }
}
