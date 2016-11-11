using GlobalPollenProject.Core.Interfaces;
using Moq;
using NUnit.Framework;

namespace GlobalPollenProject.Core.UnitTests.Factories
{
    public class TaxonFactoryTests
    {
        private Mock<ITaxonomyBackbone> _backboneMock;
        private Mock<IExternalDatabaseLinker> _linkMock;

        public TaxonFactoryTests()
        {
            _backboneMock = new Mock<ITaxonomyBackbone>();
            _backboneMock.Setup(m => m.IsValidTaxon(It.IsAny<Rank>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _linkMock = new Mock<IExternalDatabaseLinker>();
        }

        [Test]
        public void CreateTaxon_Family_FamilyNameEmpty_ReturnsNull()
        {
            
        }

        [Test]
        public void CreateTaxon_Family_FamilyExists_ReturnsNull()
        {

        }

        [Test]
        public void CreateTaxon_Family_DoesNotExist_CreatesInRepo()
        {

        }

        [Test]
        public void CreateTaxon_Genus_FamilyExists_SetsAsParent()
        {
        }

        [Test]
        public void CreateTaxon_Genus_FamilyExistsGenusExists_ReturnsNull()
        {

        }

        [Test]
        public void CreateTaxon_Genus_FamilyExistsGenusDoesNot_AddsAndReturnsGenus()
        {

        }

        [Test]
        public void CreateTaxon_Species_FamilyMissing_CreatesInRepo()
        {

        }


        [Test]
        public void CreateTaxon_Species_SetFamilyAsParentsParent()
        {

        }

        [Test]
        public void CreateTaxon_Species_GenusMissing_CreatesInRepo()
        {

        }

        [Test]
        public void CreateTaxon_Species_SetsGenusAsParent()
        {

        }

        [Test]
        public void CreateTaxon_Species_SpeciesAlreadyExists_ReturnsNull()
        {

        }

        [Test]
        public void CreateTaxon_Species_SpeciesDoesNotExist_AddsAndReturnsSpecies()
        {

        }
    }
}