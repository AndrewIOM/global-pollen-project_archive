using Im.Acm.Pollen.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.Data.Abstract;

namespace Im.Acm.Pollen.Services.Concrete
{
    public class ReferenceService : IReferenceService
    {
        private IUnitOfWork _uow;
        public ReferenceService(IUnitOfWork uow, ITaxonomyService taxonomyService)
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
            _uow.SaveChanges();
            return _uow.RefGrainRepository.GetAll().OrderBy(i => i.ReferenceGrainId).Last();
        }

        public void DeleteCollection(int collectionId)
        {
            var existing = _uow.RefCollectionRepository.GetById(collectionId);
            if (existing != null)
            {
                foreach (var grain in existing.Grains)
                {
                    _uow.RefGrainRepository.Delete(grain);
                }
                _uow.RefCollectionRepository.Delete(existing);
            }
            _uow.SaveChanges();
        }

        public void DeleteGrain(int grainId)
        {
            var existing = _uow.RefGrainRepository.GetById(grainId);
            if (existing != null)
            {
                _uow.RefGrainRepository.Delete(existing);
            }
            _uow.SaveChanges();
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

        public List<ReferenceCollection> ListCollections()
        {
            return _uow.RefCollectionRepository.GetAll().ToList();
        }
    }
}
