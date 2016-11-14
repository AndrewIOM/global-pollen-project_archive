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
            var result = new App.Models.UnknownGrain()
            {
                Id = domainGrain.Id,
                Score = domainGrain.CalculateScore(),
                ThumbnailUri = "",
                TimeAdded = domainGrain.TimeAdded,
                Latitude = domainGrain.Latitude,
                Longitude = domainGrain.Longitude,
                Status = domainGrain.ToIdentificationStatusDto(alg)
            };
            return result;
        }

        public static IdentificationStatus ToIdentificationStatusDto(this Core.UnknownGrain domainGrain, INameConfirmationAlgorithm alg)
        {
            var domainStatus = domainGrain.GetConfirmedIdentity(alg);
            var dtoStatus = new IdentificationStatus();
            dtoStatus.ConfirmedFamily = domainStatus[Core.Rank.Family];
            dtoStatus.ConfirmedGenus = domainStatus[Core.Rank.Genus];
            dtoStatus.ConfirmedSpecies = domainStatus[Core.Rank.Species];
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