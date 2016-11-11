using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Services;
using Moq;
using NUnit.Framework;

namespace GlobalPollenProject.Core.UnitTests.Services
{
    public class SimpleNameConfirmationAlgorithmTests
    {
        [Test]
        public void Family_name_agreed_when_at_least_3_people_have_70percent_agreement()
        {
            var user1 = new User("Mr", "Test", "McFirst");
            var user2 = new User("Mr", "Test", "McFirst");
            var user3 = new User("Mr", "Test", "McFirst");
            var grain = new UnknownGrain(user1, 22, 22, 1, 3000);
            grain.IdentifyAs(user1, "Pinaceae", "Pinus", "communis");
            grain.IdentifyAs(user2, "Pinaceae", "Juniperus", "communis");
            grain.IdentifyAs(user3, "Pinaceae", "Juniperus", "communis");
            var backbone = new Mock<IRepository<IBackboneTaxon>>();

            var sut = new SimpleNameConfirmationAlgorithm(backbone.Object);

            var result = sut.ConfirmName(grain);
            Assert.AreEqual("Pinaceae", result[Rank.Family]);
            Assert.AreEqual(null, result[Rank.Genus]);
            Assert.AreEqual(null, result[Rank.Species]);
        }
        
        [Test]
        public void Genus_agreed_when_family_agreed_and_when_at_least_3_people_have_70percent_agreement()
        {
            var user1 = new User("Mr", "Test", "McFirst");
            var user2 = new User("Mr", "Test", "McFirst");
            var user3 = new User("Mr", "Test", "McFirst");
            var grain = new UnknownGrain(user1, 22, 22, 1, 3000);
            grain.IdentifyAs(user1, "Pinaceae", "Juniperus", null);
            grain.IdentifyAs(user2, "Pinaceae", "Juniperus", "communis");
            grain.IdentifyAs(user3, "Pinaceae", "Juniperus", null);
            var backbone = new Mock<IRepository<IBackboneTaxon>>();

            var sut = new SimpleNameConfirmationAlgorithm(backbone.Object);

            var result = sut.ConfirmName(grain);
            Assert.AreEqual("Pinaceae", result[Rank.Family]);
            Assert.AreEqual("Juniperus", result[Rank.Genus]);
            Assert.AreEqual(null, result[Rank.Species]);
        }

        [Test]
        public void Species_agreed_when_family_and_genus_agreed_and_when_at_least_3_people_have_70percent_agreement()
        {
            var user1 = new User("Mr", "Test", "McFirst");
            var user2 = new User("Mr", "Test", "McFirst");
            var user3 = new User("Mr", "Test", "McFirst");
            var grain = new UnknownGrain(user1, 22, 22, 1, 3000);
            grain.IdentifyAs(user1, "Pinaceae", "Juniperus", "communis");
            grain.IdentifyAs(user2, "Pinaceae", "Juniperus", "communis");
            grain.IdentifyAs(user3, "Pinaceae", "Juniperus", "communis");
            var backbone = new Mock<IRepository<IBackboneTaxon>>();

            var sut = new SimpleNameConfirmationAlgorithm(backbone.Object);

            var result = sut.ConfirmName(grain);
            Assert.AreEqual("Pinaceae", result[Rank.Family]);
            Assert.AreEqual("Juniperus", result[Rank.Genus]);
            Assert.AreEqual("communis", result[Rank.Species]);
        }

        [Test]
        public void Checks_any_agreed_name_is_valid_in_taxonomic_backbone()
        {
            Assert.Fail();
        }

    }
}