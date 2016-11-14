using System;
using System.Linq;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Services
{
    public class SimpleNameConfirmationAlgorithm : INameConfirmationAlgorithm
    {
        private readonly IUnitOfWork _uow;

        public SimpleNameConfirmationAlgorithm(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IDictionary<Rank, string> ConfirmName(UnknownGrain grain)
        {
            var result = new Dictionary<Rank,string>();

            // Check Family
            var familyIds = grain.Identifications.Where(m => string.IsNullOrEmpty(m.Family)).ToList();
            if (familyIds.Count < 3)
            {
                return result;
            }
            double percentAgreementRequired = 0.70;
            var groups = familyIds.GroupBy(m => m.Family).OrderByDescending(m => m.Count());

            int allIdsCount = familyIds.Count;
            int largestCount = groups.First().Count();
            var largestName = groups.First().Key;

            double percentAgreement = (double)largestCount / (double)allIdsCount;
            if (percentAgreement < percentAgreementRequired)
            {
                return result;
            }
            result.Add(Rank.Family, largestName);

            // TODO Check Genus
            // TODO Check Species

            return result;
        }
    }
}