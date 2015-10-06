using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class Taxon
    {
        public int TaxonId { get; set; }

        [Display(Name = "Latin Name")]
        public string LatinName { get; set; }
        [Display(Name = "Common (English) Name")]
        public string CommonName { get; set; }

        public int GbifId { get; set; }
        public int NeotomaId { get; set; }
    }
}
