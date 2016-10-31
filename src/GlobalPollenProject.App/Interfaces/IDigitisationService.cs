
using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App.Interfaces
{
    public interface IDigitisationService : IAppService
    {
        // Reference Collections
        void CreateCollection (DigitisedCollection newCollection);
        void UpdateCollectionMetadata (DigitisedCollection collection);
        List<DigitisedCollection> GetCollections();
        DigitisedCollection GetCollection(int id);

        // Individual Slides
        void AddSlide (int collectionId, AddDigitisedSlide newSlide);
        void RemoveSlide (DigitisedSlide slide);
        DigitisedSlide GetSlide (int id);

        // Digitisation Rights
        void RequestDigitisationRights();
        bool HasDigitisationRights();
    }
}