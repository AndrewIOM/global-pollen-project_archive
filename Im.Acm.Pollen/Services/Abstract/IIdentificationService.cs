using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.Services.Abstract
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
