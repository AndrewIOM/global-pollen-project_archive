﻿using Xunit;

namespace GlobalPollenProject.WebUI.Tests.Unit.ViewModels
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