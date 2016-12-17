
using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class PollenProjectTaxon
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string KeyImageUrl { get; set; }
        public Rank Rank { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public int UserSubmissionsConfirmedCount { get; set; }
        public int ReferenceGrainsCount { get; set; }
        public int GbifId { get; set; }
        public int NeotomaId { get; set; }
        public List<PollenProjectTaxonSummary> SubTaxa { get; set; }
        public PollenProjectTaxonSummary ParentTaxon { get; set; }
    }
}
