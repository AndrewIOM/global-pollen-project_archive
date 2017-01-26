using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Concepts;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class UnknownGrainAggregate : IAggregate
    {   
        private UnknownGrainState _state;
        private ICollection<IDomainEvent> _events;

        public UnknownGrainAggregate(
            UserAggreagte submittedBy, 
            List<Image> images,
            double latitude, 
            double longitude,
            double maxDiameter,
            int? ageYearsBeforePresent)
        {
            _state = new UnknownGrainState();
            _events = new List<IDomainEvent>();

            // Populate Core Attributes
            _state.Images = images.Select(m => m.GetState() as ImageState).ToList();
            _state.Identifications = new List<IdentificationState>();
            _state.SubmittedBy = submittedBy.GetId();
            _state.TimeAdded = DateTime.UtcNow;
            _state.LockedScore = null;

            // Populate Metadata
            _state.SpatialContext = new Location(latitude, longitude);
            _state.MaxDiameter = maxDiameter;
            _state.AgeYearsBeforePresent = ageYearsBeforePresent;
        }

        // Behaviour

        //1. Identify
        public void Identify(Identification identification)
        {
            // A user can identify a grain that they haven't already identified
            // After this, the current identity is recomputed.
            var existing = ExistingIdentification(identification.GetUser());
            if (existing != null) throw new Exception("This user has already identified this grain");
            _state.Identifications.Add(identification.GetState() as IdentificationState);
            _events.Add(new GrainIdentified(this));
        }

        //2. Recall Identification
        public void Recall(UserId id)
        {
            // A user can recall a previous identification.
            // After this, the current identity is recomputed.
            var existing = ExistingIdentification(id);
            _state.Identifications.Remove(existing.GetState() as IdentificationState);
        }

        //3. Withdraw
        public void Withdraw()
        {
            // A user can withdraw their grain, so that it is hidden for identification.
        }

        //4. Add another Image
        public void AddImage()
        {
            // The submitter can add additional pictures.
            // Other people can then reconsider their identifications.
        }

        //5. Update Metadata
        public void UpdateMetadata()
        {  
            // The submitted can update the spatial-temporal details.
            // Doing this removes all previous identifications.
        }

        private Identification ExistingIdentification(UserId userId)
        {
            var state = _state.Identifications.FirstOrDefault(m => m.User == userId);
            if (state != null) return new Identification(state);
            return null;
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

        public object GetState()
        {
            return _state;
        }

        public ICollection<IDomainEvent> Events
        {
            get
            {
                return _events;
            }
        }

    }
}
