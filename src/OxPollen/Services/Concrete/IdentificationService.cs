using OxPollen.Data.Abstract;
using OxPollen.Models;
using OxPollen.Services.Abstract;
using OxPollen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxPollen.Services.Concrete
{
    public class IdentificationService : IIdentificationService
    {
        private IUnitOfWork _uow;
        public IdentificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Add(Identification newIdentification)
        {
            newIdentification.Family = FirstCharToUpper(newIdentification.Family);
            newIdentification.Genus = FirstCharToUpper(newIdentification.Genus);
            newIdentification.Species = FirstCharToLower(newIdentification.Species);

            _uow.IdentificationRepository.Add(newIdentification);

            var grain = _uow.GrainRepository.GetById(newIdentification.Grain.Id);
            var familyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var genusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var speciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            grain.Family = familyName;
            grain.Genus = genusName;
            grain.Species = speciesName;

            var taxonService = new TaxonomyService(_uow);
            taxonService.CreateOrUpdateTaxonomy(familyName, genusName, speciesName);

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
            var grainId = identification.Grain.Id;
            _uow.IdentificationRepository.Delete(identification);

            var grain = _uow.GrainRepository.GetById(grainId);
            var familyName = GetConfirmedName(Taxonomy.Family, grain.Identifications);
            var genusName = GetConfirmedName(Taxonomy.Genus, grain.Identifications);
            var speciesName = GetConfirmedName(Taxonomy.Species, grain.Identifications);

            grain.Family = familyName;
            grain.Genus = genusName;
            grain.Species = speciesName;

            var taxonService = new TaxonomyService(_uow);
            taxonService.CreateOrUpdateTaxonomy(familyName, genusName, speciesName);

            _uow.GrainRepository.Update(grain);
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
    }
}
