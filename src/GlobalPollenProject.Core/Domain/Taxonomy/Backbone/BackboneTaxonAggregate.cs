using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class BackboneTaxonAggregate : IAggregate
    {
        private ICollection<IDomainEvent> _events;
        private BackboneTaxonState _state;

        public BackboneTaxonAggregate(BackboneTaxonState state)
        {
            _state = state;
            _events = new List<IDomainEvent>();
        }

        public ICollection<IDomainEvent> Events { get { return _events; } }
        public object GetState() { return _state; }

        public List<string> GetNamesForAllTaxonomicRanks()
        {
            var heirarchy = new List<string> { _state.LatinName };
            var parent = _state.ParentTaxa;
            while (parent != null) 
            {
                heirarchy.Add(parent.LatinName);
                parent.ParentTaxa = parent;
            }
            return heirarchy;
        }

    }
}