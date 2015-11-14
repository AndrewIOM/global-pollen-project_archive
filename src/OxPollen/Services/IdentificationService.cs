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

        public IdentificationService()
        {
            _context = new PollenDbContext();
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
                grain.HasConfirmedIdentity = percentAgreement >= percentAgreementRequired ? true : false;
            }

            _context.SaveChanges();
        }

    }
}
