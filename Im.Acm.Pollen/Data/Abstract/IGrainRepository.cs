using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.Data.Abstract
{
    public interface IGrainRepository : IRepository<Grain>
    {
        IEnumerable<Grain> GetAllDeleted();

    }
}
