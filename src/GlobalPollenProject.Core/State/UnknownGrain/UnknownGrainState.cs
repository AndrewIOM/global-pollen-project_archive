using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.State
{
    public class UnknownGrainState : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeAdded { get; set; }
        public User SubmittedBy { get; set; }
        public List<Identification> Identifications { get; set; }
        public List<Image> Images { get; set; }
        public double? LockedScore { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MaxDiameter { get; set; }
        public int? AgeYearsBeforePresent { get; set; }
    }
}
