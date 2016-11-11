using NUnit.Framework;
using System.Linq;
using System;
using Moq;
using GlobalPollenProject.Core.Interfaces;
using System.Collections.Generic;

namespace GlobalPollenProject.Core.UnitTests
{
    public class UnknownGrainTests
    {
        [Test]
        public void Creation_sets_core_invariants()
        {
            var user = new User("Mr", "Testy", "McTest");
            var sut = new UnknownGrain(user, 1.0, 2.4, 53, 1000);
            Assert.AreEqual(sut.AgeYearsBeforePresent, 1000);
            Assert.AreEqual(sut.SubmittedBy, user);
            Assert.AreEqual(sut.Latitude, 1.0);
            Assert.AreEqual(sut.Longitude, 2.4);
            Assert.AreEqual(sut.MaxDiameter, 53);
        }        

        [Test]
        public void Add_a_user_identification()
        {
            var user = new User("Mr", "Testy", "McTest");
            var identification = new Identification("Oleaceae", "Fraxinus", "excelsior", Rank.Species, user);
            var sut = new UnknownGrain(user, 1.0, 1.0, 23, null);

            sut.IdentifyAs(identification);

            var result = sut.Identifications;
            Assert.AreEqual(1, result.Count, "Was added " + result.Count + " rather than 1 time(s).");
            Assert.AreEqual(identification, result.First());
        }

        [Test]
        public void One_identification_only_allowed_per_user()
        {
            var user = new User("Mr", "Testy", "McTest");
            var identification = new Identification("Oleaceae", "Fraxinus", "excelsior", Rank.Species, user);
            var sut = new UnknownGrain(user, 1.0, 1.0, 23, null);

            sut.IdentifyAs(identification);

            Assert.Throws<Exception>(() => sut.IdentifyAs(identification));
        }

        [Test]
        public void Remove_a_users_previous_identification()
        {
            var user = new User("Mr", "Testy", "McTest");
            var identification = new Identification("Oleaceae", "Fraxinus", "excelsior", Rank.Species, user);
            var sut = new UnknownGrain(user, 1.0, 1.0, 23, null);
            sut.IdentifyAs(identification);

            sut.RemoveIdentification(user);

            Assert.IsEmpty(sut.Identifications);
        }

        [Test]
        public void Calculate_Identity_Using_Given_Confirmation_Alogrithm()
        {
            var algorithm = new Mock<INameConfirmationAlgorithm>();
            var user = new User("Mr", "Testy", "McTest");
            var sut = new UnknownGrain(user, 1.0, 1.0, 23, null);
            var expected = new Dictionary<Rank,string>();
            expected.Add(Rank.Family, "Asteraceae");
            expected.Add(Rank.Genus, "aster");
            algorithm.Setup(m => m.ConfirmName(sut)).Returns(expected);

            var result = sut.GetConfirmedIdentity(algorithm.Object);

            Assert.AreEqual(result, expected);
        }

    }
}