using System;
using System.Collections.Generic;
using AutoMapper;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace GlobalPollenProject.App
{
    public class DigitisationService : IDigitisationService
    {
        private readonly ReferenceService _domainService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public DigitisationService(ReferenceService domainService, IMapper mapper) 
        {
            _domainService = domainService;
            _mapper = mapper;
        }

        public void AddSlide(int collectionId, AddDigitisedSlide newSlide)
        {
            throw new NotImplementedException();

            
            //Populate new Grain
            // var filesToUpload = new List<string>();
            // filesToUpload.Add(newSlide.ImageOne);
            // if (!string.IsNullOrEmpty(result.ImageTwo)) filesToUpload.Add(result.ImageTwo);
            // if (!string.IsNullOrEmpty(result.ImageThree)) filesToUpload.Add(result.ImageThree);
            // if (!string.IsNullOrEmpty(result.ImageFour)) filesToUpload.Add(result.ImageFour);
            // var uploadedFiles = await _uploadService.UploadBase64Image(filesToUpload);

            // AppUser currentUser = await UserManager.FindByNameAsync(UserManager.GetUserName(User));
            // var grain = new Grain()
            // {
            //     AgeYearsBeforePresent = result.AgeYearsBeforePresent,
            //     Latitude = result.Latitude.Value,
            //     Longitude = result.Longitude.Value,
            //     SubmittedBy = currentUser,
            //     MaxSizeNanoMetres = result.ImagesScale.Value,
            //     TimeAdded = DateTime.Now,
            //     Images = new List<GrainImage>()
            // };
            // foreach (var file in uploadedFiles)
            // {
            //     grain.Images.Add(new GrainImage()
            //     {
            //         FileName = file.Url,
            //         FileNameThumbnail = file.ThumbnailUrl
            //     });
            // }
            // _grainService.Add(grain);
        }

        public void CreateCollection(DigitisedCollection collection)
        {
            var mapped = _mapper.Map<DigitisedCollection, ReferenceCollection>(collection);
            _domainService.AddCollection(mapped);
        }

        public DigitisedCollection GetCollection(int id)
        {
            throw new NotImplementedException();
        }

        public List<DigitisedCollection> GetCollections()
        {
            throw new NotImplementedException();
        }

        public DigitisedSlide GetSlide(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveSlide(DigitisedSlide slide)
        {
            throw new NotImplementedException();
        }

        public void UpdateCollectionMetadata(DigitisedCollection collection)
        {
            throw new NotImplementedException();
        }

        public void RequestDigitisationRights()
        {
            throw new NotImplementedException();
                        // var user = _userService.GetById(UserManager.GetUserId(User));
            // user.RequestedDigitisationRights = true;
            // _userService.Update(user);

            // //Send email to all admins to let them know
            // var adminEmail = "andrew.martin@zoo.ox.ac.uk"; //temporary hack
            // _emailSender.SendEmailAsync(adminEmail, "Request for digitisation rights",
            //     user.FullName() + " has requested digitisation rights. They write: " + result.Comments).Wait();
        }

        public bool HasDigitisationRights()
        {
            throw new NotImplementedException();
        }
    }
}