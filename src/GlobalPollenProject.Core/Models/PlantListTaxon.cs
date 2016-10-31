using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.Core.Models
{
    public class PlantListTaxon
    {
        [Key]
        public int Id { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Taxonomy Rank { get; set; }
        public string LatinName { get; set; }
        public string LatinNameAuthorship { get; set; }

        //Taxonomy
        public virtual List<PlantListTaxon> ChildTaxa { get; set; }
        public virtual PlantListTaxon ParentTaxa { get; set; }
    }
}
