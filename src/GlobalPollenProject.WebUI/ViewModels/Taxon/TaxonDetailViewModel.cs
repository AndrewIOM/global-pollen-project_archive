using GlobalPollenProject.WebUI.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.ViewModels.Taxon
{
    public class TaxonDetailViewModel
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Taxonomy Rank { get; set; }

        public int GbifId { get; set; }
        public int NeotomaId { get; set; }

        public List<Models.Grain> SubmittedGrains { get; set; }
        public List<ReferenceGrain> ReferenceGrains { get; set; }

        public List<Models.Taxon> SubTaxa { get; set; }
        public Models.Taxon ParentTaxon { get; set; }
    }
}
