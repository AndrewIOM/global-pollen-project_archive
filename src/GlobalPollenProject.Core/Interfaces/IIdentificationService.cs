using System.Collections.Generic;
using GlobalPollenProject.Data.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IIdentificationService
    {
        void Add(Identification newIdentification);
        void Remove(Identification identification);

        IEnumerable<Identification> GetByGrainId(int grainId);
        Identification GetById(int id);
        IEnumerable<Identification> GetByUser(string userId);
    }
}
