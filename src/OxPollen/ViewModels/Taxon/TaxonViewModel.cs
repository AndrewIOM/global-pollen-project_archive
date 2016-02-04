using OxPollen.Models;
using System.Collections.Generic;

namespace OxPollen.ViewModels
{
    public class TaxonViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Taxonomy Rank { get; set; }
        public string ImageFilename { get; set; }
        public int UserSubmissionsConfirmedCount { get; set; }
        public int ReferenceGrainsCount { get; set; }

        public List<TaxonViewModel> Children { get; set; }
    }
}
