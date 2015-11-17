using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxPollen.Models;

namespace OxPollen.Services.Concrete
{
    public class GrainService : IGrainService
    {
        private readonly OxPollenDbContext _context;
        public GrainService(OxPollenDbContext context)
        {
            _context = context;
        }

        public void Add(PollenRecord newRecord)
        {
            _context.Add(newRecord);
            _context.SaveChanges();
        }

        public PollenRecord GetById(int id)
        {
            var result = _context.PollenRecords.FirstOrDefault(m => m.PollenRecordId == id);
            return result;
        }

        public IEnumerable<PollenRecord> GetUnidentifiedGrains()
        {
            var result = _context.PollenRecords.Where(m => m.HasConfirmedIdentity == false);
            return result;
        }
    }
}
