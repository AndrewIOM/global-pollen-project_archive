using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Identification : IEntity
    {
        private Identification() {}

        public Identification(string family, 
            string genus, 
            string species,
            Rank rank,
            User madeBy)
        {
            Time = DateTime.Now;
            Family = family;
            Genus = genus;
            Species = species;
            Rank = rank;
            User = madeBy;
        }

        public int Id { get; set; }
        public DateTime Time { get; private set; }
        public string Family { get; private set; }
        public string Genus { get; private set; }
        public string Species { get; private set; }
        public Rank Rank { get; set; }

        public User User { get; private set; }

        public bool IsDeleted { get; set; }
    }
}