using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using OxPollen.Models;
using OxPollen.Data.Concrete;
using OxPollen.Data.Abstract;
using System.Linq;
using OxPollen.Utilities;

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
            if (rank == Taxonomy.Family)
            {
                return _uow.GrainRepository.Find(m => m.IdentifiedAs == null);
            }
            else if (rank == Taxonomy.Genus)
            {
                return _uow.GrainRepository.Find(m => m.IdentifiedAs == null || m.IdentifiedAs.Rank == Taxonomy.Family);
            }
            return _uow.GrainRepository.Find(m => m.IdentifiedAs == null || m.IdentifiedAs.Rank == Taxonomy.Genus);
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

        public IEnumerable<Grain> Search(GrainSearchFilter filter)
        {
            var result = _uow.GrainRepository.GetAll();

            //Filter
            if (filter.UnknownFamily) result = result.Where(m => m.IdentifiedAs == null);
            if (filter.UnknownGenus) result = result.Where(m => m.IdentifiedAs == null || m.IdentifiedAs.Rank < Taxonomy.Genus);
            if (filter.UnknownSpecies) result = result.Where(m => m.IdentifiedAs == null || m.IdentifiedAs.Rank < Taxonomy.Species);
            result = result.Where(m => m.Latitude >= filter.LatitudeLow && m.Latitude <= filter.LatitudeHigh);
            result = result.Where(m => m.Longitude >= filter.LongitudeLow && m.Longitude <= filter.LongitudeHigh);

            //Sort
            if (filter.Sort == GrainSort.Bounty)
            {
                if (filter.Descending)
                {
                    result = result.OrderByDescending(m => BountyUtility.Calculate(m));
                } else
                {
                    result = result.OrderBy(m => BountyUtility.Calculate(m));
                }
            } else if (filter.Sort == GrainSort.Date)
            {
                if (filter.Descending)
                {
                    result = result.OrderByDescending(m => m.TimeAdded);
                }
                else
                {
                    result = result.OrderBy(m => m.TimeAdded);
                }
            }

            return result.ToList();
        }

        public void Update(Grain grain)
        {
            if (grain == null) throw new Exception("Grain cannot be null");
            _uow.GrainRepository.Update(grain);
            _uow.SaveChanges();
        }
    }
}
