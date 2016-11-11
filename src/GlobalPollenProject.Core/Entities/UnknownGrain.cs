using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class UnknownGrain : IEntity
    {
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

        // Metadata
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public double MaxDiameter { get; private set; }
        public int? AgeYearsBeforePresent { get; private set; }


        // Business logic:
        // 1. Identifications can only be made for taxa confirmed in taxonomic backbone.
        // 2. The status of taxa used for identification can change. If this occurs,
        // 2a) Clump (synonym): can move to new taxon
        // 2b) Rename (synonym): can move to new taxon
        // 2c) Divide (synonym): must reduce taxonomic resolution
        // 3. 

        // public void IdentifyAs(IBackboneTaxon taxon, User byUser)
        // {
        //     var identification = new Identification("x", "y", "z", Rank.Species, byUser);
        //     var existing = _identifications.FirstOrDefault(m => m.User == identification.User);
        //     if (existing != null) throw new Exception("This user has already identified this grain");
        //     _identifications.Add(identification);
        // }

        //TODO Implement backbone checking

        public void IdentifyAs(Identification identification)
        {
            var existing = _identifications.FirstOrDefault(m => m.User == identification.User);
            if (existing != null) throw new Exception("This user has already identified this grain");
            _identifications.Add(identification);
        }

        public void IdentifyAs(User byUser, string family, string genus, string species)
        {
            var rank = Rank.Family;
            if (string.IsNullOrEmpty(family)) return;
            if (!string.IsNullOrEmpty(genus)) rank = Rank.Genus;
            if (!string.IsNullOrEmpty(genus) && !string.IsNullOrEmpty(species)) rank = Rank.Species;
            var id = new Identification(family, genus, species, rank, byUser);
        }

        public void RemoveIdentification(User byUser)
        {
            var id = _identifications.Single(m => m.User == byUser);
            _identifications.Remove(id);
        }

        public IDictionary<Rank,string> GetConfirmedIdentity(INameConfirmationAlgorithm algorithm)
        {
            var result = algorithm.ConfirmName(this);
            return result;
        }

    }
}
