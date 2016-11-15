using System;
using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Identification : IEntity
    {
        private Identification() {}

        private Identification(string family, 
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

        public static IdentificationFactory GetFactory(ITaxonomyBackbone backbone)
        {
            return new IdentificationFactory((f,g,s,r,u) => new Identification(g,g,s,r,u), backbone);
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