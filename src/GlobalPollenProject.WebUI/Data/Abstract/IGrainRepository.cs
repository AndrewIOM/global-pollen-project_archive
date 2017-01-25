using GlobalPollenProject.WebUI.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Data.Abstract
{
    public interface IGrainRepository : IRepository<Grain>
    {
        IEnumerable<Grain> GetAllDeleted();

    }
}
