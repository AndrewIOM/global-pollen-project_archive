using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class ReferenceCollectionRepository : IRepository<ReferenceCollection>
    {
        private PollenDbContext _context;
        public ReferenceCollectionRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(ReferenceCollection entity)
        {
            _context.ReferenceCollections.Add(entity);
        }

        public void Delete(ReferenceCollection entity)
        {
            _context.ReferenceCollections.Remove(entity);
        }

        public void Edit(ReferenceCollection entity)
        {
            _context.Update(entity);
        }

        public PagedResult<ReferenceCollection> FindBy(Expression<Func<ReferenceCollection, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.ReferenceCollections.Include(m => m.Slides)
                .ThenInclude(n => n.Images).Include(m => m.Owner).Where(predicate).ToPagedList(pageNumber, pageSize);
        }

        public ReferenceCollection FirstOrDefault(Expression<Func<ReferenceCollection, bool>> predicate)
        {
            return _context.ReferenceCollections.Include(m => m.Slides)
                .ThenInclude(n => n.Images).Include(m => m.Owner).FirstOrDefault(predicate);
        }

        public PagedResult<ReferenceCollection> GetAll(int pageNumber, int pageSize)
        {
            return _context.ReferenceCollections.Include(m => m.Slides)
                .ThenInclude(n => n.Images).Include(m => m.Owner).ToPagedList(pageNumber, pageSize);
        }
    }
}
