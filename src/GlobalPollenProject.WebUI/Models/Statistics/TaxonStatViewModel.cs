
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models.Statistics
{
    public class TaxonStatViewModel
    {
        public string Name { get; set; }
        public Rank Rank { get; set; }
        public bool LinkedToNeotoma { get; set; }
    }
}
