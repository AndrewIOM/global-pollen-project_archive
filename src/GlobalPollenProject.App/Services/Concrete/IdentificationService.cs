using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.App.Mapping;
using GlobalPollenProject.Core.Imagery;
using System.Threading.Tasks;

namespace GlobalPollenProject.App.Services
{
    public class IdentificationService : IIdentificationService
    {
        private readonly IUnitOfWork _uow;
        private INameConfirmationAlgorithm _nameAlgorithm;
        private IImageProcessor _imageProcessor;
        private IUserService _userService;

        public IdentificationService(IUnitOfWork uow, 
            INameConfirmationAlgorithm nameAlgorithm, 
            IImageProcessor imageProcessor, 
            IUserService userService)
        {
            _uow = uow;
            _nameAlgorithm = nameAlgorithm;
            _imageProcessor = imageProcessor;
            _userService = userService;
        }

        public async Task<PagedAppServiceResult<UnknownGrain>> GetMyUnknownGrains(int pageSize, int page)
        {
            var result = new PagedAppServiceResult<UnknownGrain>();

            var user = await _userService.GetCurrentUser();
            if (!user.IsValid)
            {
                result.AddMessage(null, "No user is currently logged in.", AppServiceMessageType.Error);
                return result;
            }

            var domainResult = _uow.UnknownGrainRepository.FindBy(m => m.SubmittedBy.Id == user.Result.Id, page, pageSize);
            var dtoResult = domainResult.Results.Select(m => m.ToDto(_nameAlgorithm)).ToList();
            result.AddResult(dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

        public AppServiceResult<UnknownGrain> GetUnknownGrain(int grainId)
        {
            var result = new AppServiceResult<UnknownGrain>();

            var domainResult = _uow.UnknownGrainRepository.FirstOrDefault(m => m.Id == grainId);
            if (domainResult == null)
            {
                result.AddMessage("Id", "The specified grain does not exist", AppServiceMessageType.Error);
                return result;
            }

            var dtoResult = domainResult.ToDto(_nameAlgorithm);
            result.AddResult(dtoResult);
            return result;
        }

        public PagedAppServiceResult<UnknownGrain> GetUnknownGrains(GrainSearchFilter criteria, int pageSize, int page)
        {
            // TODO Remove this hack and implement criteria properly
            var domainResult = _uow.UnknownGrainRepository.GetAll(page, pageSize);
            var dtoResult = domainResult.Results.Select(m => m.ToDto(_nameAlgorithm)).ToList();
            var result = new PagedAppServiceResult<UnknownGrain>(dtoResult, domainResult.CurrentPage, domainResult.PageCount, domainResult.PageSize);
            return result;
        }

        public async Task<AppServiceResult> IdentifyAs(int grainId, string family, string genus, string species)
        {
            var result = new AppServiceResult();

            var currentUser = await _userService.GetCurrentUser();
            if (!currentUser.IsValid)
            {
                result.AddMessage("", "You must be logged in to identify a grain", AppServiceMessageType.Error);
                return result;
            }

            var grain = _uow.UnknownGrainRepository.FirstOrDefault(m => m.Id == grainId);
            if (grain == null)
            {
                result.AddMessage("", "Grain specified does not exist", AppServiceMessageType.Error);
                return result;
            }

            var currentId = grain.Identifications.FirstOrDefault(m => m.User.Id == currentUser.Result.Id);
            if (currentId != null)
            {
                result.AddMessage("", "You must remove your previous ID before submitting a new one", AppServiceMessageType.Error);
                return result;
            }

            var domainUser = currentUser.Result.ToDomainModel();
            var idFactory = Core.Identification.GetFactory(_uow.BackboneCoreService);
            var id = idFactory.TryCreateIdentification(family, genus, species, domainUser);
            if (id == null)
            {
                result.AddMessage("", "The specied taxon did not match in our taxonomic backbone. Check the name and try again.", AppServiceMessageType.Error);
                return result;
            }

            // ID was successful
            grain.IdentifyAs(id);
            _uow.UnknownGrainRepository.Edit(grain);
            _uow.SaveChanges();
            return result;
        }

        public async Task<AppServiceResult> RemoveIdentification(int grainId)
        {
            var result = new AppServiceResult();

            // Validation
            var grain = _uow.UnknownGrainRepository.FirstOrDefault(m => m.Id == grainId);
            if (grain == null)
            {
                result.AddMessage(null, "Grain specified does not exist", AppServiceMessageType.Error);
                return result;
            }
            var currentUser = await _userService.GetCurrentUser();
            if (!currentUser.IsValid)
            {
                result.AddMessage(null, "You must be logged in to identify a grain", AppServiceMessageType.Error);
                return result;
            }

            // Delete the ID
            var domainUser = currentUser.Result.ToDomainModel();
            grain.RemoveIdentification(domainUser);
            _uow.UnknownGrainRepository.Edit(grain);
            _uow.SaveChanges();
            
            return result;
        }

        public async Task<AppServiceResult> RemoveUnknownGrain(int grainId)
        {
            var result = new AppServiceResult();

            // Validation
            var grain = _uow.UnknownGrainRepository.FirstOrDefault(m => m.Id == grainId);
            if (grain == null)
            {
                result.AddMessage(null, "Grain specified does not exist", AppServiceMessageType.Error);
                return result;
            }
            var currentUser = await _userService.GetCurrentUser();
            if (!currentUser.IsValid)
            {
                result.AddMessage(null, "You must be logged in to identify a grain", AppServiceMessageType.Error);
                return result;
            }
            if (currentUser.Result.Id != grain.SubmittedBy.Id)
            {
                result.AddMessage(null, "You did not submit this grain", AppServiceMessageType.Error);
                return result;
            }
            var grainName = grain.GetConfirmedIdentity(_nameAlgorithm);
            if (grainName.Count > 0)
            {
                // It has some identity. Thus we can't remove it.
                result.AddMessage(null, "The grain has a confirmed identity and cannot be removed", AppServiceMessageType.Error);
                return result;
            }

            _uow.UnknownGrainRepository.Delete(grain);
            _uow.SaveChanges();
            result.AddMessage("", "Grain was successfully deleted", AppServiceMessageType.Info);
            return result;

        }

        public async Task<AppServiceResult> UploadUnknownGrain(AddUnknownGrain grain)
        {
            var result = new AppServiceResult();

            var loggedInUser = await _userService.GetCurrentUser();
            if (loggedInUser.Result == null)
            {
                result.AddMessage(null, "You must be logged in to upload an unknown grain", AppServiceMessageType.Error);
                return result;
            }

            var imageFactory = Core.Image.GetFactory(_imageProcessor);
            var dtoImages = new string[] {grain.ImageOne, grain.ImageTwo, grain.ImageThree, grain.ImageFour}.Where(m => !string.IsNullOrEmpty(m)).ToList();
            if (dtoImages.Count == 0)
            {
                result.AddMessage("ImageOne", "No images uploaded", AppServiceMessageType.Error);
                return result;
            }

            var base64Images = new List<Base64Image>();
            foreach (var dtoImage in dtoImages)
            {
                var base64image = Base64Image.TryCreateBase64Image(dtoImage);
                if (base64image == null)
                {
                    result.AddMessage("", "An image could not be parsed", AppServiceMessageType.Error);
                    return result;
                }
                base64Images.Add(base64image);
            }

            // Upload images
            var domainImages = new List<Core.Image>();
            foreach (var image in base64Images)
            {
                try
                {
                    var saved = await _imageProcessor.Upload(image);
                    var domainImage = await imageFactory.TryCreateImage(image);
                    domainImages.Add(domainImage);
                } catch (Exception)
                {
                    result.AddMessage("", "An internal error occured while saving your image.", AppServiceMessageType.Error);
                    return result;
                }
            }

            // Save new grain
            var user = loggedInUser.Result.ToDomainModel();
            var domainGrain = new Core.UnknownGrain(user, domainImages, grain.Latitude.Value, 
                grain.Longitude.Value, grain.ImagesScale.Value, grain.AgeYearsBeforePresent);
            _uow.UnknownGrainRepository.Add(domainGrain);
            _uow.SaveChanges();
            return new AppServiceResult();
        }

    }
}