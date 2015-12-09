using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using OxPollen.Models;
using OxPollen.Data.Concrete;
using OxPollen.Data.Abstract;

namespace OxPollen.Services.Concrete
{
    public class GrainService : IGrainService
    {
        private IUnitOfWork _uow;
        public GrainService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Grain newRecord)
        {
            _uow.GrainRepository.Add(newRecord);
            _uow.SaveChanges();
        }

        public Grain GetById(int id)
        {
            var result = _uow.GrainRepository.GetById(id);
            return result;
        }

        public IEnumerable<Grain> GetByUser(string userId)
        {
            var result = _uow.GrainRepository.Find(m => m.SubmittedBy.Id == userId);
            return result;
        }

        public IEnumerable<Grain> GetUnidentifiedGrains(Taxonomy rank)
        {
            IEnumerable<Grain> result;
            if (rank == Taxonomy.Family)
            {
                result = _uow.GrainRepository.Find(m => string.IsNullOrEmpty(m.Family));
            }
            else if (rank == Taxonomy.Genus)
            {
                result = _uow.GrainRepository.Find(m => string.IsNullOrEmpty(m.Genus));
            } else
            {
                result = _uow.GrainRepository.Find(m => string.IsNullOrEmpty(m.Species));
            }
            return result;
        }

        public Grain MarkDeleted(int id)
        {
            var result = _uow.GrainRepository.GetById(id);
            if (result == null) return null;
            result.IsDeleted = true;
            _uow.GrainRepository.Update(result);
            _uow.SaveChanges();
            return result;
        }

        public void Update(Grain grain)
        {
            if (grain == null) throw new Exception("Grain cannot be null");
            _uow.GrainRepository.Update(grain);
            _uow.SaveChanges();
        }
    }
}
