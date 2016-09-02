using Im.Acm.Pollen.Models;

namespace Im.Acm.Pollen.ViewModels.Api
{
    public class BackboneTaxon
    {
        public int Id { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Taxonomy Rank { get; set; }
        public string LatinName { get; set; }
        public string ParentLatinName { get; set; }
    }
}
