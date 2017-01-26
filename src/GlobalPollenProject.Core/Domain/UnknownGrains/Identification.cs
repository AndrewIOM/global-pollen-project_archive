using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Identification : IEntity
    {
        private ICollection<IDomainEvent> _events;
        private IdentificationState _state;

        private Identification(string family, 
            string genus, 
            string species,
            Rank rank,
            UserAggreagte madeBy)
        {
            _events = new List<IDomainEvent>();
            _state = new IdentificationState();

            _state.Time = DateTime.Now;
            _state.Family = family;
            _state.Genus = genus;
            _state.Species = species;
            _state.Rank = rank;
            _state.User = madeBy.GetId();
        }

        // Identifications can only occur when the taxon is valid *at this time* in the taxonomic backbone
        public static IdentificationFactory GetFactory(ITaxonomyBackbone backbone)
        {
            return new IdentificationFactory((f,g,s,r,u) => new Identification(g,g,s,r,u), backbone);
        }

        public object GetState() { return _state; }
        public ICollection<IDomainEvent> Events { get { return _events; } }

        public UserId GetUser()
        {
            return _state.User;
        }
    }
}