using GlobalPollenProject.WebUI.Models;

namespace GlobalPollenProject.WebUI.ViewModels.Taxon
{
    public class TaxonSuggest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
    }
}
