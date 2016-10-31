using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models.Taxon
{
    public class TaxonViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Rank Rank { get; set; }
        public string ImageFilename { get; set; }
        public int UserSubmissionsConfirmedCount { get; set; }
        public int ReferenceGrainsCount { get; set; }
        public List<TaxonChildViewModel> Children { get; set; }
    }

    public class TaxonChildViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
    }
}
