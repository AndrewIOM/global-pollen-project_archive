using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceSlide : IEntity
    {
        private ReferenceSlide() {}
        public ReferenceSlide(
            ReferenceCollection belongsTo, 
            List<Image> images,
            Taxon identifiedAs,
            double maxDiameter)
        {
            this.BelongsTo = belongsTo;
            TimeAdded = DateTime.UtcNow;
            this.Images = images;
            this.Taxon = identifiedAs;
            this.MaxDiameter = maxDiameter;
        }

        public int Id { get; set; }
        public DateTime TimeAdded { get; private set; }
        public Taxon Taxon { get; private set; }
        public double MaxDiameter { get; private set; }
        public List<Image> Images { get; private set; }
        public ReferenceCollection BelongsTo { get; private set; }

        public bool IsDeleted { get; set; }

        public string GetReference()
        {
            return "Some fake reference.";
        }
    }
}