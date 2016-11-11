
using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class PollenProjectTaxon
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public Rank Rank { get; set; }
        public string ImageFilename { get; set; }
        public int UserSubmissionsConfirmedCount { get; set; }
        public int ReferenceGrainsCount { get; set; }
        public int GbifId { get; set; }
        public int NeotomaId { get; set; }

        public List<UnknownGrain> SubmittedGrains { get; set; }
        public List<DigitisedSlide> ReferenceGrains { get; set; }

        public List<PollenProjectTaxon> SubTaxa { get; set; }
        public PollenProjectTaxon ParentTaxon { get; set; }

    }
}
