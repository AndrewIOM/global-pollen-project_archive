using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IReferenceService
    {
        //Collection
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
