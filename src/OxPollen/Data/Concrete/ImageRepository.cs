using OxPollen.Data.Abstract;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace OxPollen.Data.Concrete
{
    public class ImageRepository : IRepository<GrainImage>
    {
        private OxPollenDbContext _context;
        public ImageRepository(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(GrainImage entity)
        {
            _context.Images.Add(entity);
        }

        public void Update(GrainImage entity)
        {
            _context.Images.Update(entity);
        }

        public void Delete(GrainImage entity)
        {
            _context.Images.Remove(entity);
        }

        public IEnumerable<GrainImage> Find(Expression<Func<GrainImage, bool>> where)
        {
            return _context.Images.Where(where);
        }

        public IEnumerable<GrainImage> GetAll()
        {
            return _context.Images;
        }

        public GrainImage GetById(int id)
        {
            return _context.Images.FirstOrDefault(m => m.GrainImageId == id);
        }
    }
}
