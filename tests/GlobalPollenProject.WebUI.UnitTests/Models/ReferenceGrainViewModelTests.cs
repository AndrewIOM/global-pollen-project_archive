using Im.Acm.Pollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Im.Acm.Pollen.Tests.Unit.ViewModels
{
    public class ReferenceGrainViewModelTests
    {
        [Theory]
        [InlineData(false, "Family", "Genus", "species")]
        [InlineData(true, "Family", "", "")]
        [InlineData(false, "Family", "Genus", "")]
        [InlineData(false, "", "Genus", "")]
        [InlineData(false, "", "", "species")]
        public void Validate_FamilyRank_OnlyAllowsValuesInFamily(bool isValid, 
           string family, string genus, string species)
        {

        }
    }
}
