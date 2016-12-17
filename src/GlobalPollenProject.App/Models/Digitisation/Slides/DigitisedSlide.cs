using System;
using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class DigitisedSlide {
        public int Id { get; set; }
        public DigitisedCollectionDetail Collection { get; set; }
        public DateTime TimeAdded { get; set; }
        public PollenProjectTaxonSummary Family { get; set; }
        public PollenProjectTaxonSummary Genus { get; set; }
        public PollenProjectTaxonSummary Species { get; set; }
        public double MaxDiameter { get; set; }

        public List<Image> Images { get; set; }
    }
}