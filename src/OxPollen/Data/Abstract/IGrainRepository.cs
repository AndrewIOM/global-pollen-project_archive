using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Data.Abstract
{
    public interface IGrainRepository : IRepository<Grain>
    {
        IEnumerable<Grain> GetAllDeleted();

    }
}
