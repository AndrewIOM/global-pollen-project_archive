using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GlobalPollenProject.Data.Models.Enums;

namespace GlobalPollenProject.Data.Models
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

        //Taxonomy
        public virtual List<Taxon> ChildTaxa { get; set; }
        public virtual Taxon ParentTaxa { get; set; }

        //Grains
        public List<Grain> UserGrains { get; set; }
        public List<ReferenceGrain> ReferenceGrains { get; set; }
    }
}
