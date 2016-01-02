using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OxPollen.Models
{
    public class Taxon
    {
        [Key]
        public int TaxonId { get; set; }
        [Required]
        public string LatinName { get; set; }
        [Required]
        public Taxonomy Rank { get; set; }

        public int GbifId { get; set; }
        public int NeotomaId { get; set; }

        //Navigation Properties
        public virtual List<Grain> SubmittedGrains { get; set; }
        public virtual List<ReferenceGrain> ReferenceGrains { get; set; }
        public virtual List<Taxon> ChildTaxa { get; set; }
        public virtual Taxon ParentTaxa { get; set; }
    }
}
