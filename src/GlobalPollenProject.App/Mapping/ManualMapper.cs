using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Mapping
{
    public static class ManualMapper
    {   
        public static App.Models.UnknownGrain ToDto(this Core.UnknownGrain domainGrain, INameConfirmationAlgorithm alg)
        {
            var imageThumbnail = domainGrain.Images.Count > 0 ? domainGrain.Images.First().FileNameThumbnail : "";

            var result = new App.Models.UnknownGrain()
            {
                Id = domainGrain.Id,
                Score = domainGrain.CalculateScore(),
                ThumbnailUri = imageThumbnail,
                TimeAdded = domainGrain.TimeAdded,
                Latitude = domainGrain.Latitude,
                Longitude = domainGrain.Longitude,
                Status = domainGrain.ToIdentificationStatusDto(alg),
                Images = domainGrain.Images.Select(m => ToDto(m)).ToList(),
                MaxDiameter = domainGrain.MaxDiameter,
                Age = domainGrain.AgeYearsBeforePresent
            };
            return result;
        }

        public static App.Models.Image ToDto(this Core.Image domainImage)
        {
            var image = new App.Models.Image();
            if (domainImage.IsFocusImage)
            {
                image.ImageUrls = new List<string> {domainImage.FocusLowUrl, domainImage.FocusMedLowUrl, 
                    domainImage.FocusMedUrl, domainImage.FocusMedHighUrl, domainImage.FocusHighUrl};
            } else {
                image.ImageUrls = new List<string> { domainImage.FileName };
            }
            image.ThumbnailUrl = domainImage.FileNameThumbnail;
            return image;
        }

        public static IdentificationStatus ToIdentificationStatusDto(this Core.UnknownGrain domainGrain, INameConfirmationAlgorithm alg)
        {
            var domainStatus = domainGrain.GetConfirmedIdentity(alg);
            var dtoStatus = new IdentificationStatus();
            dtoStatus.ConfirmedFamily = domainStatus.ContainsKey(Core.Rank.Family) ? domainStatus[Core.Rank.Family] : "";
            dtoStatus.ConfirmedGenus = domainStatus.ContainsKey(Core.Rank.Genus) ? domainStatus[Core.Rank.Genus] : "";
            dtoStatus.ConfirmedSpecies = domainStatus.ContainsKey(Core.Rank.Species) ? domainStatus[Core.Rank.Species] : "";
            dtoStatus.Identifications = domainGrain.Identifications.Select(m => ToDto(m)).ToList();
            return dtoStatus;
        }

        public static IdentificationDto ToDto(this Core.Identification domainIdentification)
        {
            var dto = new IdentificationDto();
            dto.Family = domainIdentification.Family;
            dto.Genus = domainIdentification.Genus;
            dto.Species = domainIdentification.Species;
            dto.Id = domainIdentification.Id;
            dto.Rank = (App.Models.Rank) domainIdentification.Rank;
            dto.Time = domainIdentification.Time;
            dto.SubmittedBy = domainIdentification.User.UserName;
            return dto;
        }

        public static User ToDomainModel(this AppUser user)
        {
            var result = new User(user.Title, user.FirstName, user.LastName);
            result.Id = user.Id;
            return result;
        }

    }
}