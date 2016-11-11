using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Factories;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Taxon : IEntity
    {
        private Taxon(string latinName, Rank rank, Taxon parentTaxon) 
        {
            this.LatinName = latinName;
            this.Rank = rank;
            this.ParentTaxon = parentTaxon;
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

        public List<UnknownGrain> UnknownGrains { get; private set; }

        public List<ReferenceSlide> ReferenceSlides { get; private set; }

        public bool IsDeleted { get; set; }

        internal void LinkToExternalDatabases(IExternalDatabaseLinker linker)
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

            this.GbifId = linker.GetGlobalBiodiversityInformationFacilityId(family, genus, species);
            this.NeotomaId = linker.GetNeotomaDatabaseId(family, genus, species);
        }

        /// <summary>
        /// Gets the latin names for each taxonomic rank including and above the current rank.
        /// </summary>
        /// <returns>A dictionary of ranks and their latin names.</returns>
        public Dictionary<Rank,string> GetHeirarchy()
        {
            throw new NotImplementedException();
        }

    }

    public enum Rank
    {
        Family = 1,
        Genus = 2,
        Species = 3
    }
}