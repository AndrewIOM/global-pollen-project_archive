using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Services
{
    public class DigitisationService : IDigitisationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExternalDatabaseLinker _databaseLinker;
        public DigitisationService(IUnitOfWork uow, IExternalDatabaseLinker databaseLinker)
        {
            _uow = uow;
            _databaseLinker = databaseLinker;
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

        public AppServiceResult CreateCollection(DigitisedCollection newCollection)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<DigitisedCollection> GetCollection(int id)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<DigitisedCollection>> GetCollections(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<DigitisedSlide> GetSlide(int id)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult HasDigitisationRights()
        {
            throw new NotImplementedException();
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

        AppServiceResult<DigitisedCollection> IDigitisationService.CreateCollection(DigitisedCollection newCollection)
        {
            throw new NotImplementedException();
        }

        AppServiceResult<bool> IDigitisationService.HasDigitisationRights()
        {
            throw new NotImplementedException();
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