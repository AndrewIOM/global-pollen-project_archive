using System.Collections.Generic;
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
        public virtual List<Taxon> ChildTaxa { get; set; }
        public virtual Taxon ParentTaxa { get; set; }
    }
}
