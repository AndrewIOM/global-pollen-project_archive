using GlobalPollenProject.WebUI.Models;
using System;

namespace GlobalPollenProject.WebUI.Utilities
{
    public static class BountyUtility
    {
        public static double Calculate(Grain grain)
        {
            if (grain.LockedBounty.HasValue)
            {
                return grain.LockedBounty.Value;
            }

            // Four Parameter logistic growth function (score (s) on time basis)
            double s0 = 1.0;
            double r = 0.015;
            double l = 10.0;
            double k = 20.0;
            int t = (DateTime.Now - grain.TimeAdded).Days;
            double scoreAtTime = Math.Floor( l + (s0 * (k - l)) / (s0 + (k - l - s0) * Math.Exp(-r * t)) );

            // Early time supression effect to encourage quick identifications
            if (t < 5) return scoreAtTime * (1 - (0.2 * t));
            if (t >= 5 && t < 8) return scoreAtTime * (0.2 * (8 - t));
            return scoreAtTime;
        }
    }
}
