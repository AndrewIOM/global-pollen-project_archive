using System;
using System.Collections.Generic;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.App.Services
{
    public class IdentificationService : IIdentificationService
    {
        private readonly IUnitOfWork _uow;
        public IdentificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public AppServiceResult<List<UnknownGrain>> GetMyUnknownGrains(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<UnknownGrain> GetUnknownGrain(int grainId)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult<List<UnknownGrain>> GetUnknownGrains(GrainSearchFilter criteria, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult IdentifyAs(int grainId, string family, string genus, string species)
        {
            throw new NotImplementedException();
            // Identification myIdentification = null;
            // if (record != null)
            // {
            //     myIdentification = _idService.GetByUser(UserManager.GetUserId(User))
            //         .FirstOrDefault(m => m.Grain.Id == result.GrainId);
            //     if (myIdentification != null) ModelState.AddModelError("User", "You have already identified this grain, sorry!");
            // }
            // if (!_taxonomy.IsValidTaxon(result.TaxonomicResolution, result.Family, result.Genus, result.Species))
            // {
            //     ModelState.AddModelError("Family", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            // }

        }

        public AppServiceResult RemoveIdentification(int grainId)
        {
            //Check Prerequisites
            // if (existingId == null) return NotFound();
            // //TODO Stop removal if identity confirmed: if (existingId.Grain.) return BadRequest();
            // if (existingId.User.Id != UserManager.GetUserId(User)) return Unauthorized();
            
            // var grainId = existingId.Grain.Id;

            throw new NotImplementedException();
        }

        public AppServiceResult RemoveUnknownGrain(int grainId)
        {
            throw new NotImplementedException();
        }

        public AppServiceResult UploadUnknownGrain(AddUnknownGrain grain)
        {
            throw new NotImplementedException();
        }
    }
}