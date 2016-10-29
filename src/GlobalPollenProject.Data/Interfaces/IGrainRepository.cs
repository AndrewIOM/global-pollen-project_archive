using GlobalPollenProject.Data.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.Data.Interfaces
{
    public interface IGrainRepository : IRepository<Grain>
    {
        IEnumerable<Grain> GetAllDeleted();

    }
}
