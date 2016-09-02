using Im.Acm.Pollen.Data.Abstract;
using Im.Acm.Pollen.Models;
using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Im.Acm.Pollen.Services.Concrete
{
    public class IdentificationService : IIdentificationService
    {
        private IUnitOfWork _uow;
        private ITaxonomyService _taxonomyService;
        public IdentificationService(IUnitOfWork uow, ITaxonomyService taxonomyService)
        {
            _uow = uow;
            _taxonomyService = taxonomyService;
        }

        public void Add(Identification newIdentification)
        {
            var grain = _uow.GrainRepository.GetById(newIdentification.Grain.Id);
            var oldFamilyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var oldGenusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var oldSpeciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            newIdentification.Family = FirstCharToUpper(newIdentification.Family);
            newIdentification.Genus = FirstCharToUpper(newIdentification.Genus);
            newIdentification.Species = FirstCharToLower(newIdentification.Species);
            grain.Identifications.Add(newIdentification);

            var newFamilyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var newGenusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var newSpeciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            var taxon = _taxonomyService.CreateOrUpdateTaxonomy(newFamilyName, newGenusName, newSpeciesName);
            grain.IdentifiedAs = taxon;

            UpdateBountyScores(grain, oldFamilyName, newFamilyName, Taxonomy.Family);
            UpdateBountyScores(grain, oldGenusName, newGenusName, Taxonomy.Genus);
            UpdateBountyScores(grain, oldSpeciesName, newSpeciesName, Taxonomy.Species);

            _uow.GrainRepository.Update(grain);
            _uow.SaveChanges();
        }

        public IEnumerable<Identification> GetByGrainId(int grainId)
        {
            var result = _uow.GrainRepository.GetById(grainId);
            if (result != null) return result.Identifications;
            return null;
        }

        public Identification GetById(int id)
        {
            var existing = _uow.IdentificationRepository.GetById(id);
            return existing;
        }

        public IEnumerable<Identification> GetByUser(string userId)
        {
            var result = _uow.IdentificationRepository.Find(m => m.User.Id == userId);
            return result;
        }

        public void Remove(Identification identification)
        {
            var grain = _uow.GrainRepository.GetById(identification.Grain.Id);
            var oldFamilyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var oldGenusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var oldSpeciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            identification.Family = FirstCharToUpper(identification.Family);
            identification.Genus = FirstCharToUpper(identification.Genus);
            identification.Species = FirstCharToLower(identification.Species);
            grain.Identifications.Remove(identification);

            var newFamilyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var newGenusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var newSpeciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            var taxon = _taxonomyService.CreateOrUpdateTaxonomy(newFamilyName, newGenusName, newSpeciesName);
            grain.IdentifiedAs = taxon;

            UpdateBountyScores(grain, oldFamilyName, newFamilyName, Taxonomy.Family, identification);
            UpdateBountyScores(grain, oldGenusName, newGenusName, Taxonomy.Genus, identification);
            UpdateBountyScores(grain, oldSpeciesName, newSpeciesName, Taxonomy.Species, identification);

            _uow.IdentificationRepository.Delete(identification);
            _uow.SaveChanges();
        }

        private string GetConfirmedName(Taxonomy rank, List<Identification> identifications)
        {
            List<string> ids;
            if (rank == Taxonomy.Family)
            {
                ids = identifications.Where(m => !string.IsNullOrEmpty(m.Family))
                    .Select(m => m.Family).ToList();
            }
            else if (rank == Taxonomy.Genus)
            {
                ids = identifications.Where(m => !string.IsNullOrEmpty(m.Genus))
                    .Select(m => m.Genus).ToList();
            }
            else if (rank == Taxonomy.Species)
            {
                ids = identifications.Where(m => !string.IsNullOrEmpty(m.Species))
                    .Select(m => m.Species).ToList();
            }
            else
            {
                throw new Exception("Not a valid taxonomic rank");
            }

            if (ids.Count < 3) return null;
            double percentAgreementRequired = 0.70;
            var groups = ids.GroupBy(m => m).OrderByDescending(m => m.Count());

            int allIdsCount = ids.Count;
            int largestCount = groups.First().Count();
            var largestName = groups.First().Key;

            double percentAgreement = (double)largestCount / (double)allIdsCount;
            if (percentAgreement >= percentAgreementRequired)
            {
                return largestName;
            }
            return "";
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        private string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToLower() + input.Substring(1).ToLower();
        }

        private void UpdateBountyScores(Grain grain, string oldName, string newName, Taxonomy rank, Identification removedId = null)
        {
            double bounty = BountyUtility.Calculate(grain);

            if (string.IsNullOrEmpty(oldName) && !string.IsNullOrEmpty(newName))
            {
                grain.LockedBounty = bounty;
                _uow.GrainRepository.Update(grain);
            }

            if (!string.IsNullOrEmpty(oldName))
            {
                var oldValidIds = new List<Identification>();
                if (removedId != null) oldValidIds.Add(removedId);
                if (rank == Taxonomy.Family) oldValidIds.AddRange(grain.Identifications.Where(m => m.Family == oldName));
                if (rank == Taxonomy.Genus) oldValidIds.AddRange(grain.Identifications.Where(m => m.Genus == oldName));
                if (rank == Taxonomy.Species) oldValidIds.AddRange(grain.Identifications.Where(m => m.Species == oldName));
                foreach (var id in oldValidIds)
                {
                    var userId = id.User.Id;
                    var user = _uow.UserRepository.GetById(userId);
                    user.BountyScore -= bounty;
                    _uow.UserRepository.Update(user);
                }
            }

            if (!string.IsNullOrEmpty(newName))
            {
                var newValidIds = new List<Identification>();
                if (rank == Taxonomy.Family) newValidIds = grain.Identifications.Where(m => m.Family == newName).ToList();
                if (rank == Taxonomy.Genus) newValidIds = grain.Identifications.Where(m => m.Genus == newName).ToList();
                if (rank == Taxonomy.Species) newValidIds = grain.Identifications.Where(m => m.Species == newName).ToList();
                foreach (var id in newValidIds)
                {
                    var userId = id.User.Id;
                    var user = _uow.UserRepository.GetById(userId);
                    user.BountyScore += bounty;
                    _uow.UserRepository.Update(user);
                }
            }
        }
    }
}
