using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Mapping;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Imagery;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Services
{
    public class DigitisationService : IDigitisationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IImageProcessor _imageProcessor;
        private readonly IExternalDatabaseLinker _databaseLinker;
        private IUserService _userService;
        public DigitisationService(IUnitOfWork uow, IExternalDatabaseLinker databaseLinker, IUserService userService, IImageProcessor imageprocessor)
        {
            _uow = uow;
            _databaseLinker = databaseLinker;
            _userService = userService;
            _imageProcessor = imageprocessor;
        }

        public async Task<AppServiceResult> AddSlide(int collectionId, AddDigitisedSlide newSlide)
        {
            var result = new AppServiceResult();

            var currentUserResult = await _userService.GetCurrentUser();
            if (!currentUserResult.IsValid)
            {
                result.AddMessage("", "The specified collection does not exist", AppServiceMessageType.Error);
                return result;
            }

            var domainRefCollection = _uow.ReferenceCollectionRepository.FirstOrDefault(m => m.Id.Equals(newSlide.CollectionId.Value));
            if (domainRefCollection == null)
            {
                result.AddMessage("", "The specified collection does not exist", AppServiceMessageType.Error);
                return result;
            }
            if (domainRefCollection.Owner.Id != currentUserResult.Result.Id)
            {
                result.AddMessage("", "You do not own this collection", AppServiceMessageType.Error);
                return result;
            }

            var taxonFactory = Taxon.GetFactory(_uow.TaxonRepository, _uow.BackboneCoreService, _databaseLinker);
            var taxon = await taxonFactory.TryCreateTaxon(newSlide.Family, newSlide.Genus, newSlide.Species);
            if (taxon == null)
            {
                result.AddMessage("", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again", AppServiceMessageType.Error);
                return result;
            }

            // Handle Images
            var savedImages = new List<Core.Image>();
            var imageFactory = Core.Image.GetFactory(_imageProcessor);
            var staticImages = newSlide.Images.Where(m => !string.IsNullOrEmpty(m));

            // A. Static Images
            foreach (var image in staticImages)
            {
                var base64Image = Base64Image.TryCreateBase64Image(image);
                if (base64Image == null)
                {
                    result.AddMessage("", "One of the uploaded images was not encoded correctly. Please try again.", AppServiceMessageType.Error);
                    return result;
                }

                var savedImage = await imageFactory.TryCreateImage(base64Image);
                if (savedImage == null)
                {
                    result.AddMessage("", "There was an error saving your images. Please try again.", AppServiceMessageType.Error);
                    return result;
                }

                savedImages.Add(savedImage);
            }

            // B. Focus Images
            foreach (var image in newSlide.FocusImages)
            {
                var base64Images = new List<Base64Image>()
                {
                    Base64Image.TryCreateBase64Image(image.FocusLowUrl),
                    Base64Image.TryCreateBase64Image(image.FocusMedLowUrl),
                    Base64Image.TryCreateBase64Image(image.FocusMedUrl),
                    Base64Image.TryCreateBase64Image(image.FocusMedHighUrl),
                    Base64Image.TryCreateBase64Image(image.FocusHighUrl)
                }.Where(m => m != null).ToList();
                if (base64Images.Count != 5)
                {
                    result.AddMessage("", "One of the uploaded images was not encoded correctly. Please try again.", AppServiceMessageType.Error);
                    return result;
                }

                var savedImage = await imageFactory.TryCreateImage(base64Images);
                if (savedImage == null)
                {
                    result.AddMessage("", "There was an error saving your images. Please try again.", AppServiceMessageType.Error);
                    return result;
                }
                savedImages.Add(savedImage);
            }

            var slide = new ReferenceSlide(domainRefCollection, savedImages, taxon, newSlide.MaxGrainSize.Value);
            domainRefCollection.AddSlide(slide);
            _uow.ReferenceCollectionRepository.Edit(domainRefCollection);
            _uow.SaveChanges();
            return result;
        }

        public AppServiceResult<DigitisedCollection> GetCollection(int id)
        {
            var result = new AppServiceResult<DigitisedCollection>();
            var domainResult = _uow.ReferenceCollectionRepository.FirstOrDefault(m => m.Id.Equals(id));
            if (domainResult == null)
            {
                result.AddMessage("", "Specified collection does not exist", AppServiceMessageType.Error);
                return result;
            }
            result.AddResult(domainResult.ToDto());
            return result;
        }

        public PagedAppServiceResult<DigitisedCollectionDetail> GetCollections(int pageSize, int page)
        {
            var domainResult = _uow.ReferenceCollectionRepository.GetAll(page, pageSize);
            var dtoResult = domainResult.Results.Select(m => m.ToSummary()).ToList();

            var result = new PagedAppServiceResult<DigitisedCollectionDetail>
                (dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

        public AppServiceResult<DigitisedSlide> GetSlide(int id)
        {
            var result = new AppServiceResult<DigitisedSlide>();
            var domainCollection = _uow.ReferenceCollectionRepository.FirstOrDefault(m => m.Slides.Any(n => n.Id.Equals(id)));
            if (domainCollection == null)
            {
                result.AddMessage("", "The slide specified does not exist", AppServiceMessageType.Error);
                return result;
            }
            var slide = domainCollection.Slides.First(m => m.Id.Equals(id)).ToDto();
            result.AddResult(slide);
            return result;
        }

        public AppServiceResult RemoveSlide(int id)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult RequestDigitisationRights()
        {
            throw new NotImplementedException();
        }

        public AppServiceResult UpdateCollectionMetadata(DigitisedCollection collection)
        {
            // if (collection.User.Id != UserManager.GetUserId(User))
            // {
            //     return Unauthorized();
            // }

            // collection.CountryCode = model.CountryCode;
            // collection.Description = model.Description;
            // collection.FocusRegion = model.FocusRegion;
            // collection.Institution = model.Institution;
            // collection.OwnedBy = model.OwnedBy;
            // collection.Name = model.Name;
            // collection.WebAddress = model.WebAddress;
            // collection.ContactEmail = model.ContactEmail;


            throw new NotImplementedException();
        }

        public AppServiceResult<bool> HasDigitisationRights()
        {
            throw new NotImplementedException();
        }

        public async Task<AppServiceResult<DigitisedCollection>> CreateCollection(AddDigitisedCollection newCollection)
        {
            var currentUser = await _userService.GetCurrentUser();
            var domainUser = currentUser.Result.ToDomainModel();

            var domainCollection = new ReferenceCollection(domainUser);
            domainCollection.ContactEmail = newCollection.ContactEmail;
            domainCollection.CountryCode = newCollection.CountryCode;
            domainCollection.Description = newCollection.Description;
            domainCollection.FocusRegion = newCollection.FocusRegion;
            domainCollection.Institution = newCollection.Institution;
            domainCollection.Name = newCollection.Name;
            domainCollection.OwnedBy = newCollection.OwnedBy;
            domainCollection.WebAddress = newCollection.WebAddress;

            _uow.ReferenceCollectionRepository.Add(domainCollection);
            _uow.SaveChanges();

            var result = new AppServiceResult<DigitisedCollection>(domainCollection.ToDto());
            return result;
        }

        public PagedAppServiceResult<DigitisedSlide> GetSlides(int pageSize, int page, SlideSearchCriteria criteria)
        {
            var domainResult = _uow.ReferenceSlideRepository.GetSlidesForTaxon(criteria.TaxonId, true, page, pageSize);
            var dtoResult = domainResult.Results.Select(m => m.ToDto()).ToList();
            var result = new PagedAppServiceResult<DigitisedSlide>(dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

    }
}