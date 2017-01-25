using Microsoft.AspNetCore.Mvc;
using Moq;
using GlobalPollenProject.WebUI.Controllers;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Services.Abstract;
using GlobalPollenProject.WebUI.Tests.Unit.TestBuilders;
using GlobalPollenProject.WebUI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GlobalPollenProject.WebUI.Tests.Unit.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Constructor_ActivatesGrainService()
        {
            var grainService = new Mock<IGrainService>();
            var sut = new HomeController(grainService.Object);
            Assert.NotNull(sut._grainService);
        }

        [Fact]
        public void Terms_ReturnsTermsView()
        {
            var grainService = new Mock<IGrainService>();
            var sut = new HomeController(grainService.Object);
            var result = sut.Terms() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal("Terms", result.ViewName);
        }

        [Fact]
        public void Index_ReturnsIndexView()
        {
            var grainService = new Mock<IGrainService>();
            grainService.Setup(m => m.GetUnidentifiedGrains(Taxonomy.Genus)).Returns(new List<Grain>());
            var sut = new HomeController(grainService.Object);
            var result = sut.Index() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
        }

        [Fact]
        public void Index_ModelIsListOfGrains()
        {
            var grainService = new Mock<IGrainService>();
            grainService.Setup(m => m.GetUnidentifiedGrains(Taxonomy.Genus)).Returns(GrainTestBuilder.GetSampleGrains());
            var sut = new HomeController(grainService.Object);
            var result = sut.Index() as ViewResult;
            Assert.IsType<List<SimpleGrainViewModel>>(result.ViewData.Model);
        }

        [Fact]
        public void Index_ModelHasTwelveGrains()
        {
            var grainService = new Mock<IGrainService>();
            grainService.Setup(m => m.GetUnidentifiedGrains(Taxonomy.Genus)).Returns(GrainTestBuilder.GetSampleGrains());
            var sut = new HomeController(grainService.Object);
            var result = sut.Index() as ViewResult;
            var model = result.ViewData.Model as List<SimpleGrainViewModel>;
            Assert.Equal(12, model.Count);
        }

        [Fact]
        public void Index_ModelGrainsSortedByBounty()
        {
            var grainService = new Mock<IGrainService>();
            grainService.Setup(m => m.GetUnidentifiedGrains(Taxonomy.Genus)).Returns(GrainTestBuilder.GetSampleGrains());
            var sut = new HomeController(grainService.Object);
            var result = sut.Index() as ViewResult;
            var model = result.ViewData.Model as List<SimpleGrainViewModel>;
            var sorted = model.OrderByDescending(m => m.Bounty);
            Assert.Equal(sorted, model);
        }

        [Fact]
        public void Index_ModelGrainsComplete()
        {
            var grainService = new Mock<IGrainService>();
            grainService.Setup(m => m.GetUnidentifiedGrains(Taxonomy.Genus)).Returns(GrainTestBuilder.GetSampleGrains());
            var sut = new HomeController(grainService.Object);
            var result = sut.Index() as ViewResult;
            var model = result.ViewData.Model as List<SimpleGrainViewModel>;
            foreach (var grain in model)
            {
                Assert.True(grain.ThumbnailLocation != null); //Returns "" when no image
                Assert.True(grain.Id != 0);
                Assert.True(grain.Bounty > 0);
            }
        }

        [Fact]
        public void Guide_ReturnsGuideView()
        {
            var grainService = new Mock<IGrainService>();
            var sut = new HomeController(grainService.Object);
            var result = sut.Guide() as ViewResult;
            Assert.Equal("Guide", result.ViewName);
        }

        [Fact]
        public void Error_ReturnsErrorView()
        {
            var grainService = new Mock<IGrainService>();
            var sut = new HomeController(grainService.Object);
            var result = sut.Terms() as ViewResult;
            Assert.Equal("Error", result.ViewName);
        }
    }
}
