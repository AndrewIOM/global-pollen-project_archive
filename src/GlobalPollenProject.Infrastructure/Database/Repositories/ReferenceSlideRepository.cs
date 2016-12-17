using System;
using System.Linq;
using System.Linq.Expressions;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Infrastructure
{
    public class SlideRepository : ISlideRepository
    {
        private PollenDbContext _context;
        public SlideRepository(PollenDbContext context)
        {
            _context = context;
        }

        public void Add(ReferenceSlide entity)
        {
            _context.Add(entity);
        }

        public void Delete(ReferenceSlide entity)
        {
            _context.Remove(entity);
        }

        public void Edit(ReferenceSlide entity)
        {
            _context.Update(entity);
        }

        public PagedResult<ReferenceSlide> FindBy(Expression<Func<ReferenceSlide, bool>> predicate, int pageNumber, int pageSize)
        {
            return _context.Slides
                .Include(m => m.BelongsTo)
                .Include(m => m.Taxon)
                .Include(m => m.Images)            
                .Where(predicate)
                .ToPagedList(pageNumber, pageSize);
        }

        public ReferenceSlide FirstOrDefault(Expression<Func<ReferenceSlide, bool>> predicate)
        {
            return _context.Slides
                .Include(m => m.BelongsTo)
                .Include(m => m.Taxon)
                .Include(m => m.Images)            
                .FirstOrDefault(predicate);
        }

        public PagedResult<ReferenceSlide> GetAll(int pageNumber, int pageSize)
        {
            return _context.Slides
                .Include(m => m.BelongsTo)
                .Include(m => m.Taxon)
                .Include(m => m.Images)            
                .ToPagedList(pageNumber, pageSize);
        }

        public PagedResult<ReferenceSlide> GetSlidesForTaxon(int taxonId, bool recursive, int page, int pageSize)
        {
            Expression<Func<ReferenceSlide, bool>> predicate;
            var taxon = _context.Taxa.First(m => m.Id == taxonId);
            if (!recursive)
            {
                predicate = (m => m.Taxon.Id == taxonId);
            } else if (taxon.Rank == Rank.Family)
            {
                predicate = (m => m.Taxon.Id == taxonId || m.Taxon.ParentTaxon.Id == taxonId || m.Taxon.ParentTaxon.ParentTaxon.Id == taxonId);
            } else if (taxon.Rank == Rank.Genus)
            {
                predicate = (m => m.Taxon.Id == taxonId || m.Taxon.ParentTaxon.Id == taxonId);
            } else {
                predicate = (m => m.Taxon.Id == taxonId);
            }

            var match = _context.Slides
                .Include(m => m.BelongsTo)
                .Include(m => m.Taxon)
                    .ThenInclude(n => n.ParentTaxon)
                        .ThenInclude(o => o.ParentTaxon)
                .Include(m => m.Images)
                .Where(predicate)
                .ToPagedList(page, pageSize);

            return match;
        }
    }
}
