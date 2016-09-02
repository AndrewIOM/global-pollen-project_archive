using Im.Acm.Pollen.Models;
using System.Collections.Generic;

namespace Im.Acm.Pollen.Services.Abstract
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
