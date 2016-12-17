using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.State;

namespace GlobalPollenProject.Core
{
    public class Taxon : IEntity
    {
        private TaxonState _state;
        internal Taxon(TaxonState state)
        {
            _state = state;
        }

        private Taxon(string latinName, Rank rank, Taxon parentTaxon) 
        {
            _state = new TaxonState();
            _state.LatinName = latinName;
            _state.Rank = rank;
            _state.ParentTaxon = parentTaxon;
        }

        public static TaxonFactory GetFactory(IRepository<Taxon> taxonRepo, ITaxonomyBackbone backbone, IExternalDatabaseLinker linker)
        {
            return new TaxonFactory ((l,r,p) => new Taxon (l,r,p), taxonRepo, backbone, linker);
        }

        public int Id { get; set; }
        public string LatinName { get; private set; }
        public Rank Rank {get; private set; } 
        public int GbifId { get; private set; }
        public int NeotomaId { get; private set; }
        public List<Taxon> ChildTaxa { get; private set; }
        public Taxon ParentTaxon { get; private set; }
        public bool IsDeleted { get; set; }

        internal async Task LinkToExternalDatabases(IExternalDatabaseLinker linker)
        {
            string family = "";
            string genus = "";
            string species = "";
            if (this.Rank == Rank.Family)
            {
                family = this.LatinName;
            } else if (this.Rank == Rank.Genus)
            {
                genus = this.LatinName;
                family = this.ParentTaxon.LatinName;
            } else
            {
                species = this.LatinName;
                genus = this.ParentTaxon.LatinName;
                family = this.ParentTaxon.ParentTaxon.LatinName;
            }

            this.GbifId = await linker.GetGlobalBiodiversityInformationFacilityId(family, genus, species);
            this.NeotomaId = await linker.GetNeotomaDatabaseId(family, genus, species);
        }

        /// <summary>
        /// Gets the latin names for each taxonomic rank including and above the current rank.
        /// </summary>
        /// <returns>A dictionary of ranks and their latin names.</returns>
        public Dictionary<Rank,string> GetHeirarchy()
        {
            var heirarchy = new Dictionary<Rank,string>();
            heirarchy.Add(Rank.Family, "Some family");
            heirarchy.Add(Rank.Genus, "A cool genus");
            heirarchy.Add(Rank.Species, "puffinus puffinus");
            return heirarchy;
        }
    }

    public enum Rank
    {
        Family = 1,
        Genus = 2,
        Species = 3
    }
}