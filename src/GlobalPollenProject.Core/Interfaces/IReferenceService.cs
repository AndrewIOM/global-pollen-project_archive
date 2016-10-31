using System.Collections.Generic;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IReferenceService
    {
        //Collection
        List<ReferenceCollection> ListCollections();
        ReferenceCollection AddCollection(ReferenceCollection collection);
        ReferenceCollection UpdateCollection(ReferenceCollection collection);
        void DeleteCollection(int collectionId);
        ReferenceCollection GetCollectionById(int id);
        List<ReferenceCollection> GetCollectionsByUser (string userId);

        //Slides
        ReferenceGrain AddGrain(ReferenceGrain grain);
        ReferenceGrain UpdateGrain(ReferenceGrain grain);
        void DeleteGrain(int grainId);
        ReferenceGrain GetGrainById(int id);
    }
}
