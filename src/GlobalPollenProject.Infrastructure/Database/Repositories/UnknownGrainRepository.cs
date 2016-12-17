using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class UnknownGrainRepository : IUnknownGrainRepository
    {
        private PollenDbContext _context;
        public UnknownGrainRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(UnknownGrain entity)
        {
            _context.Add(entity);
        }

        public void Delete(UnknownGrain entity)
        {
            _context.Remove(entity);
        }

        public void Edit(UnknownGrain entity)
        {
            _context.Update(entity);
        }

        public PagedResult<UnknownGrain> FindBy(Expression<Func<UnknownGrain, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.UnknownGrains
                .Include(m => m.SubmittedBy)
                .Include(m => m.Identifications).ThenInclude(n => n.User)
                .Include(m => m.Images).Where(predicate)
                .ToPagedList(pageNumber, pageSize);
        }

        public UnknownGrain FirstOrDefault(Expression<Func<UnknownGrain, bool>> predicate)
        {
            return _context.UnknownGrains
                .Include(m => m.SubmittedBy)
                .Include(m => m.Identifications).ThenInclude(n => n.User)
                .Include(m => m.Images).FirstOrDefault(predicate);
        }

        public PagedResult<UnknownGrain> GetAll(int pageNumber, int pageSize)
        {
            return _context.UnknownGrains
                .Include(m => m.SubmittedBy)
                .Include(m => m.Identifications).ThenInclude(n => n.User)
                .Include(m => m.Images)
                .ToPagedList(pageNumber, pageSize);
        }

        public PagedResult<ReferenceSlide> GetGrainsIdentifiedAs(int taxonId)
        {
            throw new NotImplementedException();
        }
    }
}
