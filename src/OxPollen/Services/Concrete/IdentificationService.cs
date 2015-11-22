using Microsoft.Data.Entity;
using OxPollen.Models;
using OxPollen.Services.Abstract;
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
            var family = GetConfirmedIdentity(grain.Identifications.Select(m => m.Family).ToList());
            return family;
        }

        public string GetGenus(Grain grain)
        {
            var genus = GetConfirmedIdentity(grain.Identifications.Select(m => m.Genus).ToList());
            return genus;
        }

        public string GetSpecies(Grain grain)
        {
            var species = GetConfirmedIdentity(grain.Identifications.Select(m => m.Species).ToList());
            return species;
        }

        public void SaveIdentification(Identification newIdentification)
        {
            //TODO Link up family-genus-species
            //TODO Reinstate user bounties

            _context.Identifications.Add(newIdentification);

            //Evaluate identification status
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;
            var grain = newIdentification.Grain;
            var confirmedFamilyName = GetFamily(grain);
            if (!string.IsNullOrEmpty(confirmedFamilyName))
            {
                familyTaxon = _context.Taxa.FirstOrDefault(m => m.LatinName == confirmedFamilyName && m.Rank == Taxonomy.Family);
                if (familyTaxon == null)
                {
                    familyTaxon = new Taxon()
                    {
                        LatinName = confirmedFamilyName,
                        Rank = Taxonomy.Family,
                        Records = new List<Grain>()
                    };
                }
                familyTaxon.Records.Add(grain);
                _context.Add(familyTaxon);
            }

            var confirmedGenusName = GetGenus(grain);
            if (!string.IsNullOrEmpty(confirmedGenusName))
            {
                genusTaxon = _context.Taxa.FirstOrDefault(m => m.LatinName == confirmedGenusName && m.Rank == Taxonomy.Genus);
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
                genusTaxon.Records.Add(grain);
                _context.Add(genusTaxon);
            }

            var confirmedSpeciesName = GetSpecies(grain);
            if (!string.IsNullOrEmpty(confirmedSpeciesName))
            {
                speciesTaxon = _context.Taxa.FirstOrDefault(m => m.LatinName == confirmedSpeciesName && m.Rank == Taxonomy.Species);
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
                speciesTaxon.Records.Add(grain);
                _context.Add(speciesTaxon);
            }

            _context.SaveChanges();
        }

        //private void UpdateUserBounty(string userId, int bountyChange)
        //{
        //    var user = _context.Users.FirstOrDefault(m => m.Id == userId);
        //    if (user == null) throw new Exception("User was null!");
        //    user.BountyScore += bountyChange;
        //}

        private string GetConfirmedIdentity(List<string> ids)
        {
            if (ids.Count < 3) return null;

            int percentAgreementRequired = 100;
            var groups = ids.GroupBy(m => m);
            var percentAgreement = (groups.Count() / (percentAgreementRequired / 100)) * 100;
            if (percentAgreement >= percentAgreementRequired)
            {
                var agreedName = groups.OrderBy(m => m.Key).First().Key;
                return agreedName;
            }
            return null;
        }

        public bool IsIdentifiedByUser(int grainId, string userId)
        {
            var grainIds = _context.Identifications.Include(m => m.Grain).Include(m => m.User)
                .Where(m => m.Grain.GrainId == grainId && m.User.Id == userId);
            if (grainIds.Count() > 0) return true;
            return false;        }
    }
}
