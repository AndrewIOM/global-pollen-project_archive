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
            _context.Identifications.Add(newIdentification);
            _context.SaveChanges();
            UpdateGrainIdentificationStatus(newIdentification.Record.PollenRecordId);
        }

        public List<Identification> Get(string userId)
        {
            throw new NotImplementedException();
        }

        private void UpdateGrainIdentificationStatus(int grainId)
        {
            //TODO Remove temp fix for lack of lazy loading in beta5
            var grain = _context.PollenRecords.Include(c => c.Identifications).ToList()
                .FirstOrDefault(m => m.PollenRecordId == grainId);
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

                } else
                {
                    grain.HasConfirmedIdentity = false;
                }
            }

            _context.SaveChanges();
        }
    }
}
