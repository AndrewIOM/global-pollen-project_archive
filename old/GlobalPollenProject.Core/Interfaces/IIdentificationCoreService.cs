using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IIdentificationCoreService : ICoreService
    {
        void Add(Identification newIdentification);
        void Remove(Identification identification);

        // IEnumerable<Identification> GetByGrainId(int grainId);
        // Identification GetById(int id);
        // IEnumerable<Identification> GetByUser(string userId);
    }
}
