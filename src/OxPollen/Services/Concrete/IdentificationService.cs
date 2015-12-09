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

            CreateOrUpdateTaxonomy(familyName, genusName, speciesName);

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

            CreateOrUpdateTaxonomy(familyName, genusName, speciesName);

            _uow.GrainRepository.Update(grain);
            _uow.SaveChanges();
        }

        private string GetConfirmedName(Taxonomy rank, List<Identification> identifications)
        {
            //1. Determine Correct Name
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

            if (ids.Count < 3) return "";
            int percentAgreementRequired = 70;
            var groups = ids.GroupBy(m => m);
            var percentAgreement = (groups.Count() / (double)(percentAgreementRequired / 100)) * 100;
            if (percentAgreement >= percentAgreementRequired)
            {
                return largestName;
            }
            return "";
        }

        private void CreateOrUpdateTaxonomy(string family, string genus, string species)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            if (!string.IsNullOrEmpty(family))
            {
                familyTaxon = _uow.TaxonRepository.Find(m => m.LatinName == family && m.Rank == Taxonomy.Family).FirstOrDefault();
                if (familyTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, confirmedFamilyName, null, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(confirmedFamilyName);
                    familyTaxon = new Taxon()
                    {
                        LatinName = family,
                        Rank = Taxonomy.Family,
                        Records = new List<Grain>(),
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _uow.TaxonRepository.Add(familyTaxon);
                }
            }

            if (!string.IsNullOrEmpty(genus))
            {
                genusTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus && m.Rank == Taxonomy.Genus).FirstOrDefault();
                if (genusTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Genus,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, null);
                    genusTaxon = new Taxon()
                    {
                        LatinName = genus,
                        Rank = Taxonomy.Genus,
                        Records = new List<Grain>(),
                        ParentTaxa = familyTaxon != null ? familyTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    if (genusTaxon.ParentTaxa == null) genusTaxon.ParentTaxa = familyTaxon != null ? familyTaxon : null;
                    _uow.TaxonRepository.Add(genusTaxon);
                }
            }

            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
                speciesTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus + " " + species && m.Rank == Taxonomy.Species).FirstOrDefault();
                if (speciesTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Species,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, species);
                    speciesTaxon = new Taxon()
                    {
                        LatinName = genus + " " + species,
                        Rank = Taxonomy.Species,
                        Records = new List<Grain>(),
                        ParentTaxa = genusTaxon != null ? genusTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    if (speciesTaxon.ParentTaxa == null) speciesTaxon.ParentTaxa = genusTaxon != null ? genusTaxon : null;
                    _uow.TaxonRepository.Add(speciesTaxon);
            }
            }
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
