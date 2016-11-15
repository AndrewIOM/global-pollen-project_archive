using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class UnknownGrain : IEntity
    {
        private UnknownGrain() 
        {
            _images = new List<Image>();
            _identifications = new List<Identification>();
        }
        private readonly List<Identification> _identifications;
        private readonly List<Image> _images;

        public UnknownGrain(
            User submittedBy, 
            List<Image> images,
            double latitude, 
            double longitude,
            double maxDiameter,
            int? ageYearsBeforePresent)
        {
            // Core Attributes
            _images = images;
            _identifications = new List<Identification>();
            this.SubmittedBy = submittedBy;
            this.TimeAdded = DateTime.UtcNow;
            this.LockedScore = null;

            // Metadata
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.MaxDiameter = maxDiameter;
            this.AgeYearsBeforePresent = ageYearsBeforePresent;
        }

        // Core Attributes
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeAdded { get; private set; }
        public User SubmittedBy { get; private set; }
        public List<Identification> Identifications { get { return _identifications; } }
        public List<Image> Images { get { return _images; }}
        public double? LockedScore { get; set; }

        // Metadata
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double MaxDiameter { get; private set; }
        public int? AgeYearsBeforePresent { get; private set; }

        public void IdentifyAs(Identification identification)
        {
            var existing = _identifications.FirstOrDefault(m => m.User == identification.User);
            if (existing != null) throw new Exception("This user has already identified this grain");
            _identifications.Add(identification);
        }

        public void RemoveIdentification(User byUser)
        {
            var id = _identifications.Single(m => m.User.Id == byUser.Id);
            _identifications.Remove(id);
        }

        public IDictionary<Rank,string> GetConfirmedIdentity(INameConfirmationAlgorithm algorithm)
        {
            var result = algorithm.ConfirmName(this);
            return result;
        }

        public double CalculateScore()
        {
            if (this.LockedScore.HasValue)
            {
                return this.LockedScore.Value;
            }

            int daysSinceSubmission = (DateTime.Now - this.TimeAdded).Days;

            if (daysSinceSubmission == 0) return 6;
            if (daysSinceSubmission == 1) return 3;
            if (daysSinceSubmission == 2) return 2;
            if (daysSinceSubmission == 3) return 1;
            return 0.4 + (daysSinceSubmission * 0.2);
        }

    }
}
