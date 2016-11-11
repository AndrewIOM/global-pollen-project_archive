using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceSlide : IEntity
    {
        public ReferenceSlide(
            ReferenceCollection belongsTo, 
            List<Image> images,
            Taxon identifiedAs)
        {
            this.BelongsTo = belongsTo;
            TimeAdded = DateTime.UtcNow;
            this.Images = images;
            this.Taxon = identifiedAs;
        }

        // Core Invariants
        public int Id { get; set; }
        public DateTime TimeAdded { get; private set; }
        public Taxon Taxon { get; private set; }
        public double MaxDiameter { get; private set; }
        public List<Image> Images { get; private set; }
        public ReferenceCollection BelongsTo { get; private set; }

        public bool IsDeleted { get; set; }
    }
}