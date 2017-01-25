using GlobalPollenProject.WebUI.Data.Abstract;
using GlobalPollenProject.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GlobalPollenProject.WebUI.Data.Concrete
{
    public class ImageRepository : IRepository<GrainImage>
    {
        private PollenDbContext _context;
        public ImageRepository(PollenDbContext context)
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
