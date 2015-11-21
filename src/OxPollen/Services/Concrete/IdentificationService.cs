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
        public IdentificationService(Models.OxPollenDbContext context)
        {
            _context = context;
        }

        public void SaveIdentification(Identification newIdentification)
        {
            var bounty = newIdentification.Record.Bounty;
            _context.Identifications.Add(newIdentification);
            EvaluateGrainIdentificationStatus(newIdentification.Record.PollenRecordId);
            _context.SaveChanges();
        }

        private void UpdateUserBounty(string userId, int bountyChange)
        {
            var user = _context.Users.FirstOrDefault(m => m.Id == userId);
            if (user == null) throw new Exception("User was null!");
            user.Bounty += bountyChange;
        }

        private void EvaluateGrainIdentificationStatus(int grainId)
        {
            var grain = _context.PollenRecords.FirstOrDefault(m => m.PollenRecordId == grainId);
            if (grain == null) return;

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
                        dbUser.Bounty += grain.Bounty;
                    }
                } else
                {
                    grain.HasConfirmedIdentity = false;
                }
            }
        }
    }
}
