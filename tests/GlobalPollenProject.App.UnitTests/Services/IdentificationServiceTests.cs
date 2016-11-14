using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Services;
using GlobalPollenProject.App.Validation;
using GlobalPollenProject.Core.Interfaces;
using Moq;
using NUnit.Framework;

namespace GlobalPollenProject.App.UnitTests.Services
{
    public class IdentificationServiceTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<INameConfirmationAlgorithm> _nameAlgorithm;
        private Mock<IImageProcessor> _imageProcessor;
        private Mock<IUserService> _userService;

        [SetUp]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _nameAlgorithm = new Mock<INameConfirmationAlgorithm>();
            _imageProcessor = new Mock<IImageProcessor>();
            _userService = new Mock<IUserService>();

            //Setup user logged in
            var user = new AppUser();
            user.FirstName = "Test";
            user.LastName = "Test";
            var userResult = new AppServiceResult<AppUser>();
            userResult.AddResult(user);
            _userService.Setup(m => m.GetCurrentUser()).Returns(Task.FromResult(userResult));
        }

        [Test]
        public void GetMyUnknownGrains_UserNotLoggedIn_ReturnsError()
        {
            var expected = new AppServiceResult<AppUser>();
            expected.AddMessage("", "User not logged in", AppServiceMessageType.Error);
            _userService.Setup(m => m.GetCurrentUser()).Returns(Task.FromResult(expected));
            var sut = new IdentificationService(_uow.Object, _nameAlgorithm.Object, 
                _imageProcessor.Object, _userService.Object);

            var result = sut.GetMyUnknownGrains(1, 10).Result;

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
        }

        [Test]
        public void GetMyUnknownGrains_UserLoggedIn_GetsPagedGrains()
        {
            var sut = new IdentificationService(_uow.Object, _nameAlgorithm.Object, 
                _imageProcessor.Object, _userService.Object);

            var result = sut.GetMyUnknownGrains(1, 10).Result;

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Messages.Count);
        }

        [Test]
        public void GetUnknownGrain_NotInRepository_ReturnsError()
        {
            Assert.Fail();
        }

        [Test]
        public void GetUnknownGrain_GrainInRepository_ReturnsDto()
        {
            Assert.Fail();
        }

        [Test]
        public void IdentifyAs_GrainDoesNotExist_ReturnsError()
        {
            Assert.Fail();
        }

        [Test]
        public void IdentifyAs_FamilyNull_ReturnsError()
        {
            Assert.Fail();
        }   

        [Test]
        public void IdentifyAs_Family_SavesFamilyIdAndReturnsSuccess()
        {
            Assert.Fail();
        }

        [Test]
        public void IdentifyAs_Genus_SavesFamilyIdAndReturnsSuccess()
        {
            Assert.Fail();
        }
    }
}