using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Models.Taxon
{
    public class TaxonDetailViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Rank Rank { get; set; }

        public int GbifId { get; set; }
        public int NeotomaId { get; set; }

        // public List<Models.Grain> SubmittedGrains { get; set; }
        // public List<ReferenceGrain> ReferenceGrains { get; set; }

        // public List<Models.Taxon> SubTaxa { get; set; }
        // public Models.Taxon ParentTaxon { get; set; }
    }
}
