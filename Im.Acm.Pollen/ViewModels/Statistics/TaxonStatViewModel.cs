using Im.Acm.Pollen.Models;

namespace Im.Acm.Pollen.ViewModels.Statistics
{
    public class TaxonStatViewModel
    {
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
        public bool LinkedToNeotoma { get; set; }
    }
}
