using GlobalPollenProject.WebUI.Models;
using System.Collections.Generic;

namespace GlobalPollenProject.WebUI.Services.Abstract
{
    public interface IGrainService
    {
        void Add(Grain newRecord);
        IEnumerable<Grain> GetUnidentifiedGrains(Taxonomy rank);
        IEnumerable<Grain> Search(GrainSearchFilter filter);
        Grain GetById(int id);
        IEnumerable<Grain> GetByUser(string userId);
        Grain MarkDeleted(int id);
        void Update(Grain grain);
    }
}
