using System.Threading.Tasks;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Interfaces
{
    public interface IDigitisationService : IAppService
    {
        // Reference Collections
        Task<AppServiceResult<DigitisedCollection>> CreateCollection (AddDigitisedCollection newCollection);
        AppServiceResult UpdateCollectionMetadata (DigitisedCollection collection);
        PagedAppServiceResult<DigitisedCollectionDetail> GetCollections(int pageSize, int page);
        AppServiceResult<DigitisedCollection> GetCollection(int id);

        // Individual Slides
        Task<AppServiceResult> AddSlide (int collectionId, AddDigitisedSlide newSlide);
        AppServiceResult RemoveSlide (int id);
        AppServiceResult<DigitisedSlide> GetSlide (int id);
        PagedAppServiceResult<DigitisedSlide> GetSlides (int pageSize, int page, SlideSearchCriteria criteria);

        // Digitisation Rights
        AppServiceResult RequestDigitisationRights();
        AppServiceResult<bool> HasDigitisationRights();
    }
}