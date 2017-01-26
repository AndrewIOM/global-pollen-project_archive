using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Concepts;

namespace GlobalPollenProject.Core
{
    public class UnknownGrainState
    {
        // Identity
        public UnknownGrainId Id { get; set; }

        // Core State
        public bool IsDeleted { get; set; }
        public DateTime TimeAdded { get; set; }
        public UserId SubmittedBy { get; set; }
        public List<ImageState> Images { get; set; }

        public Location SpatialContext { get; set; }
        public double MaxDiameter { get; set; }


        // Optional State
        public List<IdentificationState> Identifications { get; set; }
        public TaxonId IdentifiedAs { get; set; }
        public double? LockedScore { get; set; }
        public int? AgeYearsBeforePresent { get; set; }
    }
}
