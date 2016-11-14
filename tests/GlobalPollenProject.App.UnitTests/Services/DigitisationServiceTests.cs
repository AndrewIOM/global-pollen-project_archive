using Moq;
using NUnit.Framework;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.App.Services;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App.UnitTests
{
    public class DigitisationServiceTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IExternalDatabaseLinker> _linker;

        public DigitisationServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _linker = new Mock<IExternalDatabaseLinker>();
        }

        [Test]
        public void AddSlide_UserNotLoggedIn_ReturnsError()
        {
            // var slide = new AddDigitisedSlide();
            // var sut = new DigitisationService(_uowMock.Object, _linker.Object);
            // sut.AddSlide()
        }

        [Test]
        public void AddSlide_UserDoesNotOwnCollection_ReturnsError()
        {

        }

        [Test]
        public void AddSlide_ErrorCreatingTaxon_ReturnsError()
        {

        }

        [Test]
        public void AddSlide_ErrorUploadingImage_ReturnsError()
        {

        }

        [Test]
        public void AddSlide_ValidNewSlide_AddsSlideToDatabase()
        {

        }


    }
}