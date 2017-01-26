using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.Core;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Mapping
{
    public static class ManualMapper
    { 
        public static App.Models.UnknownGrain ToDto(this Core.UnknownGrainAggregate domainGrain, INameConfirmationAlgorithm alg)
        {
            var state = domainGrain.GetState() as UnknownGrainState;
            var imageThumbnail = state.Images.Count > 0 ? state.Images.First().FileNameThumbnail : "";

            var result = new App.Models.UnknownGrain()
            {
                Id = state.Id.Id,
                Score = domainGrain.CalculateScore(),
                ThumbnailUri = imageThumbnail,
                TimeAdded = state.TimeAdded,
                Latitude = state.SpatialContext.Latitude,
                Longitude = state.SpatialContext.Longitude,
                Status = domainGrain.ToIdentificationStatusDto(alg),
                Images = state.Images.Select(m => ToDto(m)).ToList(),
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

        public static User ToDomainModel(this AppUser user) // TODO Retire method
        {
            var result = new User(user.Title, user.FirstName, user.LastName);
            result.Id = user.Id;
            return result;
        }

        public static AppUser ToDto(this User user)
        {
            var result = new AppUser();
            result.Email = user.Email;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.Id = user.Id;
            result.ScoreTotal = user.BountyScore;
            result.Title = user.Title;
            return result;
        }

        public static DigitisedCollection ToDto(this ReferenceCollection collection)
        {
            var result = new DigitisedCollection();
            result.Id = collection.Id;
            result.ContactEmail = collection.ContactEmail;
            result.CountryCode = collection.CountryCode;
            result.Description = collection.Description;
            result.FocusRegion = collection.FocusRegion;
            result.Institution = collection.Institution;
            result.Name = collection.Name;
            result.UserId = collection.Owner.Id;
            result.WebAddress = collection.WebAddress;
            result.Slides = collection.Slides.Select(m => m.ToDto()).ToList();
            return result;
        }

        public static DigitisedCollectionDetail ToSummary(this ReferenceCollection collection)
        {
            var result = new DigitisedCollectionDetail();
            result.Id = collection.Id;
            result.ContactEmail = collection.ContactEmail;
            result.CountryCode = collection.CountryCode;
            result.Description = collection.Description;
            result.FocusRegion = collection.FocusRegion;
            result.Institution = collection.Institution;
            result.Name = collection.Name;
            result.WebAddress = collection.WebAddress;
            result.SlidesCount = collection.Slides.Count;
            return result;
        }

        public static DigitisedSlide ToDto(this ReferenceSlide slide)
        {
            var result = new DigitisedSlide();
            result.Collection = slide.BelongsTo.ToSummary();
            result.Id = slide.Id;
            result.Images = slide.Images.Select(m => m.ToDto()).ToList();
            result.MaxDiameter = slide.MaxDiameter;
            result.TimeAdded = slide.TimeAdded;

            // if (slide.Taxon.Rank == Core.Rank.Family)
            // {
            //     result.Family = slide.Taxon.ToSummary();
            // } else if (slide.Taxon.Rank == Core.Rank.Genus)
            // {
            //     result.Family = slide.Taxon.ParentTaxon.ToSummary();
            //     result.Genus = slide.Taxon.ToSummary();
            // } else {
            //     result.Family = slide.Taxon.ParentTaxon.ParentTaxon.ToSummary();
            //     result.Genus = slide.Taxon.ParentTaxon.ToSummary();
            //     result.Species = slide.Taxon.ToSummary();
            // }

            return result;
        }

        public static PollenProjectTaxonSummary ToSummary(this Taxon domainTaxon)
        {
            var result = new PollenProjectTaxonSummary()
            {
                LatinName = domainTaxon.LatinName,
                Id = domainTaxon.Id
            };
            return result;
        }

        public static PollenProjectTaxon ToDto(this Taxon domainTaxon)
        {
            var result = new PollenProjectTaxon()
            {
                Id = domainTaxon.Id,
                LatinName = domainTaxon.LatinName,
                Rank = (App.Models.Rank) domainTaxon.Rank,
                UserSubmissionsConfirmedCount = 1,
                ReferenceGrainsCount = 1,
                GbifId = domainTaxon.GbifId,
                NeotomaId = domainTaxon.NeotomaId,
                SubTaxa = domainTaxon.ChildTaxa.Select(m => m.ToSummary()).ToList(),
                ParentTaxon = domainTaxon.ParentTaxon == null ? null : domainTaxon.ParentTaxon.ToSummary(),
                Family = GetName(domainTaxon, App.Models.Rank.Family),
                Genus = GetName(domainTaxon, App.Models.Rank.Genus),
                Species = GetName(domainTaxon, App.Models.Rank.Species)
            };
            
            return result;
        }

        private static string GetName(Taxon taxon, App.Models.Rank rank)
        {
            string family = "";
            string genus = "";
            string species = "";
            if (taxon.Rank == Core.Rank.Family)
            {
                family = taxon.LatinName;
            }
            if (taxon.Rank == Core.Rank.Genus)
            {
                genus = taxon.LatinName;
                family = taxon.ParentTaxon.LatinName;
            }
            if (taxon.Rank == Core.Rank.Species)
            {
                species = taxon.LatinName;
                genus = taxon.ParentTaxon.LatinName;
                family = taxon.ParentTaxon.ParentTaxon.LatinName;
            }

            if (rank == App.Models.Rank.Family) return family;
            if (rank == App.Models.Rank.Genus) return genus;
            if (rank == App.Models.Rank.Species) return species;
            return "";
        }

        public static BackboneTaxon ToDto(this BackboneTaxonAggregate domainTaxon)
        {
            var result = new BackboneTaxon();
            result.Id = domainTaxon.Id;
            result.ParentLatinName = domainTaxon.ParentTaxa == null ? "" : domainTaxon.ParentTaxa.LatinName;
            result.LatinName = domainTaxon.LatinName;
            result.Rank = (App.Models.Rank)domainTaxon.Rank;
            result.Status = (App.Models.TaxonomicStatus)domainTaxon.Status;
            result.LatinNameAuthorship = domainTaxon.LatinNameAuthorship;
            result.Reference = domainTaxon.Reference;
            result.ReferenceUrl = domainTaxon.ReferenceUrl;
            return result;
        }
        
    }
}