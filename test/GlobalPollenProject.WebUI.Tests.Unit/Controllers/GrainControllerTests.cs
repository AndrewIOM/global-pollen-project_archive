using Microsoft.AspNetCore.Mvc;
using Moq;
using GlobalPollenProject.WebUI.Controllers;
using GlobalPollenProject.WebUI.Services.Abstract;
using System;
using Xunit;

namespace GlobalPollenProject.WebUI.Tests.Unit.Controllers
{
    public class GrainControllerTests
    {
        private Mock<IGrainService> _grainService;
        private Mock<IIdentificationService> _idService;
        private Mock<IServiceProvider> _services;
        private Mock<IFileStoreService> _fileService;
        private Mock<ITaxonomyBackbone> _backbone;

        public GrainControllerTests()
        {
            _grainService = new Mock<IGrainService>();
            _idService = new Mock<IIdentificationService>();
            _services = new Mock<IServiceProvider>();
            _fileService = new Mock<IFileStoreService>();
            _backbone = new Mock<ITaxonomyBackbone>();
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var accountController = new GrainController(_idService.Object, _grainService.Object,
                _services.Object, _fileService.Object, _backbone.Object);
            var result = accountController.Index() as ViewResult;
            Assert.Equal("Index", result.ViewName);
        }
    }
}
