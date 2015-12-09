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
                .Where(m => !string.IsNullOrEmpty(m.Family)).Select(m => m.Family).ToList());
            return family;
        }

        public string GetGenus(Grain grain)
        {
            var genus = GetConfirmedIdentity(grain.Identifications
                .Where(m => !string.IsNullOrEmpty(m.Genus)).Select(m => m.Genus).ToList());
            return genus;
        }

        public string GetSpecies(Grain grain)
        {
            var species = GetConfirmedIdentity(grain.Identifications
                .Where(m => !string.IsNullOrEmpty(m.Species)).Select(m => m.Species).ToList());
            return species;
        }

        public void SaveIdentification(Identification newIdentification)
        {
            var grain = _context.UserGrains.FirstOrDefault(m => m.GrainId == newIdentification.Grain.GrainId);
            newIdentification.Family = FirstCharToUpper(newIdentification.Family);
            newIdentification.Genus = FirstCharToUpper(newIdentification.Genus);
            newIdentification.Species = FirstCharToLower(newIdentification.Species);
            _context.Identifications.Add(newIdentification);
            EvaluateIdentifications(grain);
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
            var grain = _context.UserGrains.FirstOrDefault(m => m.GrainId == identification.Grain.GrainId);
            EvaluateIdentifications(grain);
            _context.SaveChanges();
        }

        private void EvaluateIdentifications(Grain grain)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            var confirmedFamilyName = GetFamily(grain);
            if (!string.IsNullOrEmpty(confirmedFamilyName))
            {
                familyTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedFamilyName && m.Rank == Taxonomy.Family);
                if (familyTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, confirmedFamilyName, null, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(confirmedFamilyName);
                    familyTaxon = new Taxon()
                    {
                        LatinName = confirmedFamilyName,
                        Rank = Taxonomy.Family,
                        Records = new List<Grain>(),
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _context.Add(familyTaxon);
                }
            }

            var confirmedGenusName = GetGenus(grain);
            if (!string.IsNullOrEmpty(confirmedGenusName))
            {
                genusTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedGenusName && m.Rank == Taxonomy.Genus);
                if (genusTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Genus,
                        familyTaxon != null ? familyTaxon.LatinName : null, confirmedGenusName, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(confirmedGenusName);
                    genusTaxon = new Taxon()
                    {
                        LatinName = confirmedGenusName,
                        Rank = Taxonomy.Genus,
                        Records = new List<Grain>(),
                        ParentTaxa = familyTaxon != null ? familyTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _context.Add(genusTaxon);
                }
            }

            var confirmedSpeciesName = GetSpecies(grain);
            if (!string.IsNullOrEmpty(confirmedSpeciesName) && !string.IsNullOrEmpty(confirmedGenusName))
            {
                speciesTaxon = _context.Taxa
                    .Include(m => m.Records)
                    .FirstOrDefault(m => m.LatinName == confirmedGenusName + " " + confirmedSpeciesName && m.Rank == Taxonomy.Species);
                if (speciesTaxon == null)
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Species,
                        familyTaxon != null ? familyTaxon.LatinName : null, confirmedGenusName, confirmedSpeciesName);
                    var neotomaId = NeotomaUtility.GetTaxonId(confirmedSpeciesName);
                    speciesTaxon = new Taxon()
                    {
                        LatinName = confirmedGenusName + " " + confirmedSpeciesName,
                        Rank = Taxonomy.Species,
                        Records = new List<Grain>(),
                        ParentTaxa = genusTaxon != null ? genusTaxon : null,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _context.Add(speciesTaxon);
                }
            }

            //Update linkages between grain and taxa
            if (!string.IsNullOrEmpty(grain.Family))
            {
                var oldTaxa = _context.Taxa.Include(m => m.Records).FirstOrDefault(m => m.LatinName == grain.Family && m.Rank == Taxonomy.Family);
                if (oldTaxa != null) oldTaxa.Records.Remove(grain);
            }
            if (!string.IsNullOrEmpty(confirmedFamilyName) && familyTaxon != null) familyTaxon.Records.Add(grain);
            if (!string.IsNullOrEmpty(grain.Genus))
            {
                var oldTaxa = _context.Taxa.Include(m => m.Records).FirstOrDefault(m => m.LatinName == grain.Genus && m.Rank == Taxonomy.Genus);
                if (oldTaxa != null) oldTaxa.Records.Remove(grain);
            }
            if (!string.IsNullOrEmpty(confirmedGenusName) && genusTaxon != null) genusTaxon.Records.Add(grain);
            if (!string.IsNullOrEmpty(grain.Species))
            {
                var oldTaxa = _context.Taxa.Include(m => m.Records).FirstOrDefault(m => m.LatinName == grain.Species && m.Rank == Taxonomy.Species);
                if (oldTaxa != null) oldTaxa.Records.Remove(grain);
            }
            if (!string.IsNullOrEmpty(confirmedSpeciesName) && speciesTaxon != null) speciesTaxon.Records.Add(grain);

            //Update cached identification on grain DbObject
            grain.Family = familyTaxon == null ? "" : familyTaxon.LatinName;
            grain.Genus = genusTaxon == null ? "" : genusTaxon.LatinName;
            grain.Species = speciesTaxon == null ? "" : speciesTaxon.LatinName;

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
