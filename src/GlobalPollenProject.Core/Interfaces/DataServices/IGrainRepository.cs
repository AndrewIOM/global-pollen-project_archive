using GlobalPollenProject.Core.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IGrainRepository : IRepository<Grain>
    {
        IEnumerable<Grain> GetAllDeleted();

    }
}
