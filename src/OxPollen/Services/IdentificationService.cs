using Microsoft.Data.Entity;
using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services
{
    public class IdentificationService
    {
        private readonly PollenDbContext _context;

        public IdentificationService(PollenDbContext context)
        {
            _context = context;
        }

        public void SaveIdentification(Identification newIdentification)
        {
            var bounty = newIdentification.Record.Bounty;
            _context.Identifications.Add(newIdentification);
            _context.SaveChanges();
            UpdateGrainIdentificationStatus(newIdentification.Record.PollenRecordId);

            ////Handle bounty
            //var usersCorrectlyIdentified 
            //UpdateUserBounty(newIdentification.UserId, bounty);
        }

        private void UpdateUserBounty(string userId, int bountyChange)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user == null) throw new Exception("User was null!");
            user.Bounty += bountyChange;
            _context.SaveChanges();
        }

        private void UpdateGrainIdentificationStatus(int grainId)
        {
            //TODO Remove temp fix for lack of lazy loading in beta5
            var grain = _context.PollenRecords.Include(c => c.Identifications).ToList()
                .FirstOrDefault(m => m.PollenRecordId == grainId);
            if (grain == null) return;

            var grainBounty = grain.Bounty;
            var totalIdentifications = grain.Identifications.Count;
            if (totalIdentifications < 3)
            {
                grain.HasConfirmedIdentity = false;
            }
            else
            {
                int percentAgreementRequired = 100;
                var groups = grain.Identifications.GroupBy(m => m.TaxonName);
                var percentAgreement = (groups.Count() / (percentAgreementRequired / 100)) * 100;

                if (percentAgreement >= percentAgreementRequired)
                {
                    grain.HasConfirmedIdentity = true;
                    grain.TimeIdentityConfirmed = DateTime.Now;

                    //Add or Update Taxon
                    var agreedName = groups.OrderBy(m => m.Key).First().Key;
                    var existingTaxon = _context.Taxa.FirstOrDefault(m => string.Equals(m.LatinName, agreedName, StringComparison.OrdinalIgnoreCase));
                    if (existingTaxon == null)
                    {
                        //Create new taxon and link
                        var taxon = new Taxon()
                        {
                            CommonName = "",
                            LatinName = agreedName
                        };
                        taxon.Records = new List<PollenRecord>();
                        taxon.Records.Add(grain);
                        _context.Taxa.Add(taxon);
                    } else
                    {
                        //Update existing taxon
                        existingTaxon.Records.Add(grain);
                    }

                    //Update User Bounties
                    var usersWithCorrectAnswer = grain.Identifications.Where(m => m.TaxonName == agreedName).Select(m => m.UserId);
                    foreach (var user in usersWithCorrectAnswer)
                    {
                        var dbUser = _context.Users.FirstOrDefault(m => m.Id == user);
                        if (dbUser == null) throw new Exception("Problem updating bounties");
                        dbUser.Bounty += grainBounty;
                    }
                } else
                {
                    grain.HasConfirmedIdentity = false;
                }
            }

            _context.SaveChanges();
        }
    }
}
