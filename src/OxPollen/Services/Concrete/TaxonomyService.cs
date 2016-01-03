using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using OxPollen.Models;
using OxPollen.Data.Abstract;
using OxPollen.Utilities;
using System.Linq;

namespace OxPollen.Services.Concrete
{
    public class TaxonomyService : ITaxonomyService
    {
        private IUnitOfWork _uow;
        public TaxonomyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void CreateOrUpdateTaxonomy(string family, string genus, string species)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            if (!string.IsNullOrEmpty(family))
            {
                family = FirstCharToUpper(family);
                familyTaxon = _uow.TaxonRepository.Find(m => m.LatinName == family && m.Rank == Taxonomy.Family).FirstOrDefault();
                if (familyTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, family, null, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(family);
                    familyTaxon = new Taxon()
                    {
                        LatinName = family,
                        Rank = Taxonomy.Family,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _uow.TaxonRepository.Add(familyTaxon);
                }
            }

            if (!string.IsNullOrEmpty(genus))
            {
                genus = FirstCharToUpper(genus);
                genusTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus && m.Rank == Taxonomy.Genus).FirstOrDefault();
                if (genusTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Genus,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(genus);
                    genusTaxon = new Taxon()
                    {
                        LatinName = genus,
                        Rank = Taxonomy.Genus,
                        ParentTaxa = familyTaxon != null ? familyTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    if (genusTaxon.ParentTaxa == null) genusTaxon.ParentTaxa = familyTaxon != null ? familyTaxon : null;
                    _uow.TaxonRepository.Add(genusTaxon);
                }
            }

            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
                species = FirstCharToUpper(species);
                speciesTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus + " " + species && m.Rank == Taxonomy.Species).FirstOrDefault();
                if (speciesTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Species,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, species);
                    var neotomaId = NeotomaUtility.GetTaxonId(genus + " " + species);
                    speciesTaxon = new Taxon()
                    {
                        LatinName = genus + " " + species,
                        Rank = Taxonomy.Species,
                        ParentTaxa = genusTaxon != null ? genusTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    if (speciesTaxon.ParentTaxa == null) speciesTaxon.ParentTaxa = genusTaxon != null ? genusTaxon : null;
                    _uow.TaxonRepository.Add(speciesTaxon);
                }
            }
        }

        public IEnumerable<Taxon> GetAll()
        {
            var result = _uow.TaxonRepository.GetAll();
            return result;
        }

        public IEnumerable<Taxon> GetAll(Taxonomy rank)
        {
            var result = _uow.TaxonRepository.Find(m => m.Rank == rank);
            return result;
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        private string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToLower() + input.Substring(1).ToLower();
        }

        public Taxon GetById(int id)
        {
            return _uow.TaxonRepository.GetById(id);
        }

        public IEnumerable<ReferenceGrain> GetReferenceGrains(Taxon taxon)
        {
            IEnumerable<ReferenceGrain> result = new List<ReferenceGrain>();
            if (taxon.Rank == Taxonomy.Species) result = _uow.RefGrainRepository.Find(m => m.Species == taxon.LatinName);
            else if (taxon.Rank == Taxonomy.Genus) result = _uow.RefGrainRepository.Find(m => m.Genus == taxon.LatinName);
            else result = _uow.RefGrainRepository.Find(m => m.Family == taxon.LatinName);
            return result;
        }

        public IEnumerable<Grain> GetUserGrains(Taxon taxon)
        {
            IEnumerable<Grain> result = new List<Grain>();
            if (taxon.Rank == Taxonomy.Species) result = _uow.GrainRepository.Find(m => m.Species == taxon.LatinName);
            else if (taxon.Rank == Taxonomy.Genus) result = _uow.GrainRepository.Find(m => m.Genus == taxon.LatinName);
            else result = _uow.GrainRepository.Find(m => m.Family == taxon.LatinName);
            return result;
        }
    }
}
