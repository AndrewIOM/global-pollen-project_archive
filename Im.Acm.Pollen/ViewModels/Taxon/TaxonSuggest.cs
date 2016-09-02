using Im.Acm.Pollen.Models;

namespace Im.Acm.Pollen.ViewModels.Taxon
{
    public class TaxonSuggest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
    }
}
