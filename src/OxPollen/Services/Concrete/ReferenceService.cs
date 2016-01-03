using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxPollen.Models;
using OxPollen.Data.Abstract;

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
            grain.Family = FirstCharToUpper(grain.Family);
            grain.Genus = FirstCharToUpper(grain.Genus);
            grain.Species = FirstCharToLower(grain.Species);

            _uow.RefGrainRepository.Add(grain);
            var taxonService = new TaxonomyService(_uow);
            taxonService.CreateOrUpdateTaxonomy(grain.Family, grain.Genus, grain.Species);
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
