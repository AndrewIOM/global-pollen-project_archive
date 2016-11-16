using System;
using System.Collections.Generic;

namespace GlobalPollenProject.App.Models
{
    public class DigitisedSlide {
        public List<Image> Images { get; set; }

        public int Id { get; set; }
        public DateTime TimeAdded { get; set; }
        public PollenProjectTaxon Taxon { get; set; }
        public double MaxDiameter { get; set; }
        public DigitisedCollection BelongsTo { get; set; }
    }
}