using OxPollen.Models;

namespace OxPollen.ViewModels
{
    public class TaxonViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Taxonomy Rank { get; set; }

        public int UserSubmissionsConfirmedCount { get; set; }
        public int ReferenceGrainsCount { get; set; }
    }
}
