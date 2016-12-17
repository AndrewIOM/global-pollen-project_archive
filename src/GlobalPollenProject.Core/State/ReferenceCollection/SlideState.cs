using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.State
{
    public class SlideState : IEntity
    {
        public int Id { get; set; }
        public DateTime TimeAdded { get; set; }
        public double MaxDiameter { get; set; }
        public bool IsDeleted { get; set; }

        public List<ImageState> Images { get; set; }
        public ReferenceCollectionState BelongsTo { get; set; }
        public TaxonState Taxon { get; set; }
    }
}