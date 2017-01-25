using GlobalPollenProject.WebUI.Models;

namespace GlobalPollenProject.WebUI.ViewModels.Statistics
{
    public class TaxonStatViewModel
    {
        public string Name { get; set; }
        public Taxonomy Rank { get; set; }
        public bool LinkedToNeotoma { get; set; }
    }
}
