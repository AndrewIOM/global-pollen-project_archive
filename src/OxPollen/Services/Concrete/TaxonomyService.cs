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

            if (familyTaxon == null) return;
            if (!string.IsNullOrEmpty(genus))
            {
                genus = FirstCharToUpper(genus);
                genusTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus 
                    && m.Rank == Taxonomy.Genus && m.ParentTaxa.LatinName == family).FirstOrDefault();
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

            if (genusTaxon == null) return;
            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
                species = FirstCharToLower(species);
                speciesTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus + " " + species && m.Rank == Taxonomy.Species
                    && m.ParentTaxa.LatinName == genus).FirstOrDefault();
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
            if (taxon.Rank == Taxonomy.Species) result = _uow.RefGrainRepository.Find(m => m.Genus + " " + m.Species == taxon.LatinName && m.Genus == taxon.ParentTaxa.LatinName);
            else if (taxon.Rank == Taxonomy.Genus) result = _uow.RefGrainRepository.Find(m => m.Genus == taxon.LatinName && m.Family == taxon.ParentTaxa.LatinName);
            else result = _uow.RefGrainRepository.Find(m => m.Family == taxon.LatinName);
            return result;
        }

        public IEnumerable<Grain> GetUserGrains(Taxon taxon)
        {
            IEnumerable<Grain> result = new List<Grain>();
            if (taxon.Rank == Taxonomy.Species) result = _uow.GrainRepository.Find(m => m.Genus + " " + m.Species == taxon.LatinName && m.Genus == taxon.ParentTaxa.LatinName);
            else if (taxon.Rank == Taxonomy.Genus) result = _uow.GrainRepository.Find(m => m.Genus == taxon.LatinName && m.Family == taxon.ParentTaxa.LatinName);
            else result = _uow.GrainRepository.Find(m => m.Family == taxon.LatinName);
            return result;
        }

        public void RemoveTaxon(int id)
        {
            var taxon = _uow.TaxonRepository.GetById(id);
            if (taxon == null) return;
            _uow.TaxonRepository.Delete(taxon);
            _uow.SaveChanges();
        }

        public string GetRandomImageForTaxon(int id)
        {
            var taxon = _uow.TaxonRepository.GetById(id);
            if (taxon == null) return null;
            var grains = GetUserGrains(taxon).ToList();
            if (grains.Count > 0)
            {
                return grains.First().Images.First().FileNameThumbnail;
            }

            var refGrains = GetReferenceGrains(taxon).ToList();
            if (refGrains.Count > 0)
            {
                return refGrains.First().Images.First().FileNameThumbnail;
            }

            return null;
        }
    }
}
