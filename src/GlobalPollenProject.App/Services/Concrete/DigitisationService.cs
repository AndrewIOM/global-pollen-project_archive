using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Mapping;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Services
{
    public class DigitisationService : IDigitisationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExternalDatabaseLinker _databaseLinker;
        private IUserService _userService;
        public DigitisationService(IUnitOfWork uow, IExternalDatabaseLinker databaseLinker, IUserService userService)
        {
            _uow = uow;
            _databaseLinker = databaseLinker;
            _userService = userService;
        }

        public AppServiceResult AddSlide(int collectionId, AddDigitisedSlide newSlide)
        {
            throw new NotImplementedException();

            // var result = new AppServiceResult();

            // var taxonFactory = Taxon.GetFactory(_uow.TaxonRepository, _uow.BackboneCoreService, _databaseLinker);
            // var taxon = taxonFactory.TryCreateTaxon(newSlide.Family, newSlide.Genus, newSlide.Species);
            // if (taxon == null)
            // {
            //     result.Messages.Add(new AppServiceMessage(null, "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again", AppServiceMessageType.Error));
            //     return result;
            // }

            // if (!_backbone.IsValidTaxon(result.Rank, result.Family, result.Genus, result.Species))
            // {
            //     ModelState.AddModelError("TaxonomicBackbone", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            // }

            // foreach (var image in result.Images)
            // {
            //     if (!string.IsNullOrEmpty(image)) if (!IsBase64String(image)) ModelState.AddModelError("Images", "There was an encoding error when uploading your image. Please try a different image, or report the problem.");
            // }
        }

        public AppServiceResult<DigitisedCollection> GetCollection(int id)
        {
            var result = new AppServiceResult<DigitisedCollection>();
            var domainResult = _uow.ReferenceCollectionRepository.FirstOrDefault(m => m.Id == id);
            if (domainResult == null)
            {
                result.AddMessage("", "Specified collection does not exist", AppServiceMessageType.Error);
                return result;
            }
            result.AddResult(domainResult.ToDto());
            return result;
        }

        public AppServiceResult<List<DigitisedCollection>> GetCollections(int pageSize, int page)
        {
            var domainResult = _uow.ReferenceCollectionRepository.GetAll(page, pageSize);
            var result = new AppServiceResult<List<DigitisedCollection>>(domainResult.Results.Select(m => m.ToDto()).ToList());
            return result;
        }

        public AppServiceResult<DigitisedSlide> GetSlide(int id)
        {
            var result = new AppServiceResult<DigitisedSlide>();
            var domainCollection = _uow.ReferenceCollectionRepository.FirstOrDefault(m => m.Slides.Any(n => n.Id == id));
            if (domainCollection == null)
            {
                result.AddMessage("", "The slide specified does not exist", AppServiceMessageType.Error);
                return result;
            }
            var slide = domainCollection.Slides.First(m => m.Id == id).ToDto();
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

        // private string GetName(Taxonomy rank, ReferenceGrain grain)
        // {
        //     string species = null;
        //     string genus = null;
        //     string family = null;
        //     if (grain.Taxon != null)
        //     {
        //         if (grain.Taxon.Rank == Taxonomy.Species)
        //         {
        //             species = grain.Taxon.LatinName;
        //             genus = grain.Taxon.ParentTaxa.LatinName;
        //             family = grain.Taxon.ParentTaxa.ParentTaxa.LatinName;
        //         }
        //         else if (grain.Taxon.Rank == Taxonomy.Genus)
        //         {
        //             genus = grain.Taxon.LatinName;
        //             family = grain.Taxon.ParentTaxa.LatinName;
        //         }
        //         else
        //         {
        //             family = grain.Taxon.LatinName;
        //         }
        //     }
        //     if (rank == Taxonomy.Species) return species;
        //     if (rank == Taxonomy.Genus) return genus;
        //     return family;
        // }

    }
}