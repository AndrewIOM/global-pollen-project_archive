using Moq;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Services;
using GlobalPollenProject.WebUI.Services.Abstract;
using System;
using Xunit;

namespace GlobalPollenProject.WebUI.Tests.Unit.Controllers
{
    public class ReferenceControllerTests
    {
        private Mock<IFileStoreService> _fileService;
        private Mock<IReferenceService> _refService;
        private Mock<IUserService> _userService;
        private Mock<IEmailSender> _emailSender;
        private Mock<ITaxonomyBackbone> _backbone;

        public ReferenceControllerTests()
        {
            _fileService = new Mock<IFileStoreService>();
            _refService = new Mock<IReferenceService>();
            _userService = new Mock<IUserService>();
            _emailSender = new Mock<IEmailSender>();
            _backbone = new Mock<ITaxonomyBackbone>();
        }

        [Fact]
        public void AddGrain_Get_NotDigitiser_Unauthorised()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AddGrain_Get_InvalidCollectionId_BadRequest()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void AddGrain_Get_ValidCollectionIdButNotOwner_Unauthorised()
        {

        }

        [Fact]
        public void AddGrain_Get_ValidCollectionIdAndOwner_GetsCollectionOnce()
        {

        }

        [Fact]
        public void AddGrain_Get_ValidCollectionIdAndOwner_ReturnsViewWithCollectionAsModel()
        {

        }

        [Fact]
        public void AddGrain_Post_NotDigitiser_Unauthorised()
        {

        }

        [Fact]
        public void AddGrain_Post_NotCollectionOwner_Unauthorised()
        {

        }

        [Fact]
        public void AddGrain_Post_TaxonNotValidInBackbone_AddsModelError()
        {

        }

        [Fact]
        public void AddGrain_Post_ImagesNotBase64_AddsModelErrors()
        {

        }

        [Fact]
        public void AddGrain_Post_InvalidModel_BadRequest()
        {

        }

        [Theory]
        [InlineData("Poaceae", "Fraxinus", "Excelsior", Taxonomy.Species)]
        [InlineData("Poaceae", "", "", Taxonomy.Family)]
        [InlineData("Poaceae", "Fraxinus", "", Taxonomy.Genus)]
        public void AddGrain_Post_AssignsTaxonomicRank(string family, string genus, string species, Taxonomy realRank)
        {
        }

        [Fact]
        public void AddGrain_Post_ValidModelState_UploadsFiles()
        {

        }

        [Fact]
        public void Index_ReturnsView()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Index_GetsCollectionsOnce()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Index_AllCollectionsInModel()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Collection_InvalidId_ReturnsNotFound()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Collection_ValidId_ReturnsView()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Collection_ValidId_ViewModelIsCollection()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Grain_InvalidId_ReturnsNotFound()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Grain_ValidId_GetsCorrectGrainOnce()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Grain_ValidId_ReturnsView()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Grain_ValidId_ViewModelIsGrain()
        {
            throw new NotImplementedException();

        }

        [Fact]
        public void Help_ReturnsView()
        {
            throw new NotImplementedException();
        }
    }

}
