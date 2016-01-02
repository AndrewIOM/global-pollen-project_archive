using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxPollen.Models;
using OxPollen.Data.Abstract;
using OxPollen.Utilities;

namespace OxPollen.Services.Concrete
{
    public class ReferenceService : IReferenceService
    {
        private IUnitOfWork _uow;
        public ReferenceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ReferenceCollection AddCollection(ReferenceCollection collection)
        {
            _uow.RefCollectionRepository.Add(collection);
            _uow.SaveChanges();
            return _uow.RefCollectionRepository.GetAll().OrderBy(i => i.Id).Last();
        }

        public ReferenceGrain AddGrain(ReferenceGrain grain)
        {
            _uow.RefGrainRepository.Add(grain);
            CreateOrUpdateTaxonomy(grain.Family, grain.Genus, grain.Species, grain);
            _uow.SaveChanges();
            return _uow.RefGrainRepository.GetAll().OrderBy(i => i.ReferenceGrainId).Last();
        }

        public void DeleteCollection(int collectionId)
        {
            var existing = _uow.RefCollectionRepository.GetById(collectionId);
            if (existing != null)
            {
                _uow.RefCollectionRepository.Delete(existing);
            }
        }

        public void DeleteGrain(int grainId)
        {
            var existing = _uow.RefGrainRepository.GetById(grainId);
            if (existing != null)
            {
                _uow.RefGrainRepository.Delete(existing);
            }
        }

        public ReferenceCollection GetCollectionById(int id)
        {
            return _uow.RefCollectionRepository.GetById(id);
        }

        public List<ReferenceCollection> GetCollectionsByUser(string userId)
        {
            return _uow.RefCollectionRepository.Find(m => m.User.Id == userId).ToList();
        }

        public ReferenceGrain GetGrainById(int id)
        {
            return _uow.RefGrainRepository.GetById(id);
        }

        public ReferenceCollection UpdateCollection(ReferenceCollection collection)
        {
            _uow.RefCollectionRepository.Update(collection);
            _uow.SaveChanges();
            return _uow.RefCollectionRepository.GetById(collection.Id);
        }

        public ReferenceGrain UpdateGrain(ReferenceGrain grain)
        {
            _uow.RefGrainRepository.Update(grain);
            _uow.SaveChanges();
            return _uow.RefGrainRepository.GetById(grain.ReferenceGrainId);
        }

        private void CreateOrUpdateTaxonomy(string family, string genus, string species, ReferenceGrain grain)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            if (!string.IsNullOrEmpty(family))
            {
                familyTaxon = _uow.TaxonRepository.Find(m => m.LatinName == family && m.Rank == Taxonomy.Family).FirstOrDefault();
                if (familyTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, family, null, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(family);
                    familyTaxon = new Taxon()
                    {
                        LatinName = family,
                        Rank = Taxonomy.Family,
                        SubmittedGrains = new List<Grain>(),
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result,
                        ReferenceGrains= new List<ReferenceGrain>()
                    };
                    familyTaxon.ReferenceGrains.Add(grain);
                    _uow.TaxonRepository.Add(familyTaxon);
                }
            }

            if (!string.IsNullOrEmpty(genus))
            {
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
                        SubmittedGrains = new List<Grain>(),
                        ParentTaxa = familyTaxon != null ? familyTaxon : null,
                        GbifId = gbifID.Result,
                        ReferenceGrains = new List<ReferenceGrain>()
                    };
                    genusTaxon.ReferenceGrains.Add(grain);
                    if (genusTaxon.ParentTaxa == null) genusTaxon.ParentTaxa = familyTaxon != null ? familyTaxon : null;
                    _uow.TaxonRepository.Add(genusTaxon);
                }
            }

            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
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
                        SubmittedGrains = new List<Grain>(),
                        ParentTaxa = genusTaxon != null ? genusTaxon : null,
                        GbifId = gbifID.Result,
                        ReferenceGrains = new List<ReferenceGrain>()
                    };
                    speciesTaxon.ReferenceGrains.Add(grain);
                    if (speciesTaxon.ParentTaxa == null) speciesTaxon.ParentTaxa = genusTaxon != null ? genusTaxon : null;
                    _uow.TaxonRepository.Add(speciesTaxon);
                }
            }
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
    }
}
