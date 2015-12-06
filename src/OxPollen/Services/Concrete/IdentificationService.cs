using Microsoft.Data.Entity;
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
        private readonly OxPollenDbContext _context;
        public IdentificationService(OxPollenDbContext context)
        {
            _context = context;
        }

        public bool HasConfirmedIdentity(Grain grain)
        {
            var family = GetFamily(grain);
            var genus = GetGenus(grain);
            var species = GetSpecies(grain);
            if (!string.IsNullOrEmpty(family) || !string.IsNullOrEmpty(genus)
                || !string.IsNullOrEmpty(species))
                return true;
            return false;
        }

        public string GetFamily(Grain grain)
        {
            var family = GetConfirmedIdentity(grain.Identifications
                .Where(m => m.Family != null).Select(m => m.Family).ToList());
            return family;
        }

        public string GetGenus(Grain grain)
        {
            var genus = GetConfirmedIdentity(grain.Identifications
                .Where(m => m.Genus != null).Select(m => m.Genus).ToList());
            return genus;
        }

        public string GetSpecies(Grain grain)
        {
            var species = GetConfirmedIdentity(grain.Identifications
                .Where(m => m.Species != null).Select(m => m.Species).ToList());
            return species;
        }

        public void SaveIdentification(Identification newIdentification)
        { 
            var grain = _context.UserGrains.FirstOrDefault(m => m.GrainId == newIdentification.Grain.GrainId);

            //1. Standardise casing and add new ID
            newIdentification.Family = FirstCharToUpper(newIdentification.Family);
            newIdentification.Genus = FirstCharToUpper(newIdentification.Genus);
            newIdentification.Species = FirstCharToLower(newIdentification.Species);
            _context.Identifications.Add(newIdentification);

            //2. Evaluate All IDs for status
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            //2a) Family
            var confirmedFamilyName = GetFamily(grain);
            if (!string.IsNullOrEmpty(confirmedFamilyName))
            {
                familyTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedFamilyName && m.Rank == Taxonomy.Family);
                if (familyTaxon == null)
                {
                    familyTaxon = new Taxon()
                    {
                        LatinName = confirmedFamilyName,
                        Rank = Taxonomy.Family,
                        Records = new List<Grain>()
                    };
                }
                var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, confirmedFamilyName, null, null);
                familyTaxon.GbifId = gbifID.Result;
                familyTaxon.Records.Add(grain);
                _context.Add(familyTaxon);
            }

            var confirmedGenusName = GetGenus(grain);
            if (!string.IsNullOrEmpty(confirmedGenusName))
            {
                genusTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedGenusName && m.Rank == Taxonomy.Genus);
                if (genusTaxon == null)
                {
                    genusTaxon = new Taxon()
                    {
                        LatinName = confirmedGenusName,
                        Rank = Taxonomy.Genus,
                        Records = new List<Grain>(),
                        ParentTaxa = familyTaxon != null ? familyTaxon : null
                    };
                }
                var gbifID = GbifUtility.GetGbifId(Taxonomy.Genus,
                    familyTaxon != null ? familyTaxon.LatinName : null, confirmedGenusName, null);
                genusTaxon.GbifId = gbifID.Result;
                genusTaxon.Records.Add(grain);
                _context.Add(genusTaxon);
            }

            var confirmedSpeciesName = GetSpecies(grain);
            if (!string.IsNullOrEmpty(confirmedSpeciesName))
            {
                speciesTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedSpeciesName && m.Rank == Taxonomy.Species);
                if (speciesTaxon == null)
                {
                    speciesTaxon = new Taxon()
                    {
                        LatinName = confirmedGenusName + " " + confirmedSpeciesName,
                        Rank = Taxonomy.Species,
                        Records = new List<Grain>(),
                        ParentTaxa = genusTaxon != null ? genusTaxon : null
                    };
                }
                var gbifID = GbifUtility.GetGbifId(Taxonomy.Species,
                    familyTaxon != null ? familyTaxon.LatinName : null, confirmedGenusName, confirmedSpeciesName);
                speciesTaxon.GbifId = gbifID.Result;
                speciesTaxon.Records.Add(grain);
                _context.Add(speciesTaxon);
            }

            //Update cached identification on grain DbObject
            grain.Family = familyTaxon == null ? "" : familyTaxon.LatinName;
            grain.Genus = genusTaxon == null ? "" : genusTaxon.LatinName;
            grain.Species = speciesTaxon == null ? "" : speciesTaxon.LatinName;

            _context.SaveChanges();
        }

        private void UpdateUserBounty(string userId, int bountyChange)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user == null) throw new Exception("User was null!");
            user.BountyScore += bountyChange;
        }

        private string GetConfirmedIdentity(List<string> ids)
        {
            if (ids.Count < 3) return null;
            int percentAgreementRequired = 70;
            var groups = ids.GroupBy(m => m);
            var percentAgreement = (groups.Count() / (double)(percentAgreementRequired / 100)) * 100;
            if (percentAgreement >= percentAgreementRequired)
            {
                var agreedName = groups.OrderBy(m => m.Key).First().Key;
                return agreedName;
            }
            return null;
        }

        public Identification GetUsersIdentification(int grainId, string userId)
        {
            var result = _context.Identifications.Include(m => m.Grain).Include(m => m.User)
                .FirstOrDefault(m => m.Grain.GrainId == grainId && m.User.Id == userId);
            return result;
        }

        public Identification GetById(int id)
        {
            var existing = _context.Identifications.Include(m => m.Grain)
                .Include(m => m.User)
                .FirstOrDefault(m => m.IdentificationId == id);
            return existing;
        }

        public void Remove(Identification identification)
        {
            _context.Identifications.Remove(identification);
            _context.SaveChanges();
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
