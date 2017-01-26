using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;

namespace GlobalPollenProject.Core
{
    public class Taxon : IAggregate, IEntity
    {
        private TaxonState _state;
        private ICollection<IDomainEvent> _events;
        private Taxon(string latinName, Rank rank, Taxon parentTaxon) 
        {
            _events = new List<IDomainEvent>();
            _state = new TaxonState();
            _state.LatinName = latinName;
            _state.Rank = rank;
            _state.ParentTaxon = parentTaxon.GetState() as TaxonState;
        }

        public TaxonId GetId()
        {
            return _state.Id;
        }

        public static TaxonFactory GetFactory(IRepository<Taxon> taxonRepo, ITaxonomyBackbone backbone, IExternalDatabaseLinker linker)
        {
            return new TaxonFactory ((l,r,p) => new Taxon (l,r,p), taxonRepo, backbone, linker);
        }

        public ICollection<IDomainEvent> Events { get { return _events; } }
        public object GetState() { return _state; }

        public string LatinNameAtRank(Rank ofRank)
        {
            var currentRank = _state.Rank;
            throw new NotImplementedException();
        }

        internal async Task CreateThirdPartyLinks(IExternalDatabaseLinker linker)
        {
            string family = "";
            string genus = "";
            string species = "";
            if (_state.Rank == Core.Rank.Family)
            {
                family = _state.LatinName;
            } else if (_state.Rank == Core.Rank.Genus)
            {
                genus = _state.LatinName;
                family = _state.ParentTaxon.LatinName;
            } else
            {
                species = _state.LatinName;
                genus = _state.ParentTaxon.LatinName;
                family = _state.ParentTaxon.ParentTaxon.LatinName;
            }

            _state.GbifId = await linker.GetGlobalBiodiversityInformationFacilityId(family, genus, species);
            _state.NeotomaId = await linker.GetNeotomaDatabaseId(family, genus, species);
        }
    }
}