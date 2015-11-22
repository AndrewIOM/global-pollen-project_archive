using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Utilities
{
    public static class BountyUtility
    {
        public static double Calculate(DateTime timeAdded)
        {
            int daysSinceSubmission = (DateTime.Now - timeAdded).Days;
            if (daysSinceSubmission == 0) return 2.5;
            return daysSinceSubmission + 1;
        }
    }
}
