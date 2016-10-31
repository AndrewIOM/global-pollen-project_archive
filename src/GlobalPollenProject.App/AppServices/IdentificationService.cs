using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace GlobalPollenProject.App
{
    public class IdentificationService : IIdentificationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentificationManager _idManager;

        public IdentificationService(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        public UnknownGrain GetUnknownGrain(int grainId)
        {
            // Identification myIdentification = null;
            // if (User.Identity.IsAuthenticated)
            // {
            //     myIdentification = _idService.GetByUser(UserManager.GetUserId(User))
            //         .FirstOrDefault(m => m.Grain.Id == id);
            // }

            // var viewModel = new IdentificationFormViewModel()
            // {
            //     AlreadyIdentifiedByUser = myIdentification != null,
            //     UserIdentification = myIdentification,
            //     GrainId = record.Id,
            //     Age = record.AgeYearsBeforePresent,
            //     IdentifiedFamily = GetIdentifiedName(Taxonomy.Family, record),
            //     IdentifiedGenus = GetIdentifiedName(Taxonomy.Genus, record),
            //     IdentifiedSpecies = GetIdentifiedName(Taxonomy.Species, record),
            //     ImageUrls = record.Images.Select(m => m.FileName).ToList(),
            //     Latitude = record.Latitude,
            //     Longitude = record.Longitude,
            //     TimeAdded = record.TimeAdded,
            //     ImageScale = record.MaxSizeNanoMetres,
            //     Identifications = record.Identifications
            // };

            throw new NotImplementedException();
        }

        public List<UnknownGrain> GetUnknownGrains(GrainSearchFilter criteria)
        {
            throw new NotImplementedException();

            // var simpleGrains = grains.Select(m => new SimpleGrainViewModel()
            // {
            //     Bounty = BountyUtility.Calculate(m),
            //     Id = m.Id,
            //     TimeAdded = m.TimeAdded,
            //     ImageLocation = m.Images.Count > 0 ? m.Images.First().FileName : null,
            //     ThumbnailLocation = m.Images.Count > 0 ? m.Images.First().FileNameThumbnail : null,
            //     Latitude = m.Latitude,
            //     Longitude = m.Longitude
            // }).ToList();
        }

        public void IdentifyAs (int grainId, string family, string genus, string species)
        {
            throw new NotImplementedException();
            // //Save identification
            // AppUser currentUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
            // var identification = new Identification()
            // {
            //     Family = result.Family,
            //     Genus = result.Genus,
            //     Species = result.Species,
            //     Grain = record,
            //     Time = DateTime.Now,
            //     User = currentUser,
            //     Rank = result.TaxonomicResolution
            // };
            // _idService.Add(identification);

        }

        public void RemoveIdentification(int grainId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUnknownGrain(int grainId)
        {
            throw new NotImplementedException();
        }

        public void UploadUnknownGrain(AddUnknownGrain grain)
        {
            throw new NotImplementedException();
        }

        public List<UnknownGrain> GetMyUnknownGrains()
        {
            throw new NotImplementedException();

            // var thisUser = UserManager.GetUserId(User);
            
            // var grains = _grainService.GetByUser(thisUser).ToList();
            // var model = grains.Select(m => new SimpleGrainViewModel()
            // {
            //     Id = m.Id,
            //     ImageLocation = m.Images.First().FileName,
            //     Bounty = BountyUtility.Calculate(m),
            //     TimeAdded = m.TimeAdded,
            //     ConfirmedFamily = GetIdentifiedName(Taxonomy.Family, m),
            //     ConfirmedGenus = GetIdentifiedName(Taxonomy.Genus, m),
            //     ConfirmedSpecies = GetIdentifiedName(Taxonomy.Species, m),
            // }).ToList();
        }
    }
}