using Microsoft.AspNet.Mvc;
using Moq;
using OxPollen.Controllers;
using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OxPollen.Tests.Unit.Controllers
{
    public class GrainControllerTests
    {
        private Mock<IGrainService> _grainService;
        private Mock<IIdentificationService> _idService;
        private Mock<IServiceProvider> _services;
        private Mock<IFileStoreService> _fileService;

        public GrainControllerTests()
        {
            _grainService = new Mock<IGrainService>();
            _idService = new Mock<IIdentificationService>();
            _services = new Mock<IServiceProvider>();
            _fileService = new Mock<IFileStoreService>();
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var accountController = new GrainController(_idService.Object, _grainService.Object,
                _services.Object, _fileService.Object);
            var result = accountController.Index() as ViewResult;
            Assert.Equal("Index", result.ViewName);
        }
    }
}
