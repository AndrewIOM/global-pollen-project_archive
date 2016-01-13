using OxPollen.Models;
using System;

namespace OxPollen.Utilities
{
    public static class BountyUtility
    {
        public static double Calculate(Grain grain)
        {
            if (grain.LockedBounty.HasValue)
            {
                return grain.LockedBounty.Value;
            }

            int daysSinceSubmission = (DateTime.Now - grain.TimeAdded).Days;

            if (daysSinceSubmission == 0) return 4;
            if (daysSinceSubmission == 1) return 3;
            if (daysSinceSubmission == 2) return 2;
            if (daysSinceSubmission == 3) return 1;
            return 0.4 + (daysSinceSubmission * 0.2);
        }
    }
}
