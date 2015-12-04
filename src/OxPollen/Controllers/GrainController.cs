using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using OxPollen.Models;
using OxPollen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using OxPollen.Services.Abstract;
using OxPollen.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace OxPollen.Controllers
{
    public class GrainController : Controller
    {
        private IGrainService _grainService;
        private IIdentificationService _idService;
        private IFileStoreService _uploadService;
        public UserManager<AppUser> UserManager { get; set; }
        public GrainController(IIdentificationService id, IGrainService grain,
            IServiceProvider services, IFileStoreService fileService)
        {
            _idService = id;
            _grainService = grain;
            _uploadService = fileService;
            UserManager = services.GetRequiredService<UserManager<AppUser>>();
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new GrainViewModel();
            return View(model);
        }

        private bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(GrainViewModel result)
        {
            //Validation
            //if (!string.IsNullOrEmpty(result.ImageOne)) if (!IsBase64String(result.ImageOne)) ModelState.AddModelError("ImageOne", "Image not ecoded in base64");
            //if (!string.IsNullOrEmpty(result.ImageTwo)) if (!IsBase64String(result.ImageTwo)) ModelState.AddModelError("ImageTwo", "Image not ecoded in base64");
            //if (!string.IsNullOrEmpty(result.ImageThree)) if (!IsBase64String(result.ImageThree)) ModelState.AddModelError("ImageThree", "Image not ecoded in base64");
            //if (!string.IsNullOrEmpty(result.ImageFour)) if (!IsBase64String(result.ImageFour)) ModelState.AddModelError("ImageFour", "Image not ecoded in base64");
            if (ModelState.ErrorCount > 0)
            {
                return HttpBadRequest(ModelState);
            }

            //Populate new Grain
            var filesToUpload = new List<string>();
            filesToUpload.Add(result.ImageOne);
            if (!string.IsNullOrEmpty(result.ImageTwo)) filesToUpload.Add(result.ImageTwo);
            if (!string.IsNullOrEmpty(result.ImageThree)) filesToUpload.Add(result.ImageThree);
            if (!string.IsNullOrEmpty(result.ImageFour)) filesToUpload.Add(result.ImageFour);
            var uploadedFiles = _uploadService.Upload(filesToUpload);

            AppUser currentUser = await UserManager.FindByNameAsync(User.GetUserName());
            var grain = new Grain()
            {
                AgeYearsBeforePresent = result.AgeYearsBeforePresent,
                Latitude = result.Latitude.Value,
                Longitude = result.Longitude.Value,
                SubmittedBy = currentUser,
                TimeAdded = DateTime.Now,
                Images = new List<GrainImage>()
            };
            foreach (var file in uploadedFiles)
            {
                grain.Images.Add(new GrainImage()
                {
                    FileName = file,
                    ScaleNanoMetres = result.ImagesScale.Value
                });
            }
            _grainService.Add(grain);
            return Ok();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var grains = _grainService.GetUnidentifiedGrains();
            var model = grains
                .OrderByDescending(m => BountyUtility.Calculate(m.TimeAdded)).Select(m => new ReadOnlyGrainViewModel()
                {
                    Bounty = BountyUtility.Calculate(m.TimeAdded),
                    Id = m.GrainId,
                    TimeAdded = m.TimeAdded,
                    ImageLocation = m.Images.Count > 0 ? m.Images.First().FileName : null
                }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            var record = _grainService.GetById(id);
            if (record == null) return RedirectToAction("Index");

            Identification myIdentification = null;
            if (User.IsSignedIn())
            {
                myIdentification = _idService.GetUsersIdentification(record.GrainId, User.GetUserId());
            }

            var viewModel = new IdentificationViewModel()
            {
                AlreadyIdentifiedByUser = myIdentification != null,
                UserIdentification = myIdentification,
                GrainId = record.GrainId,
                Age = record.AgeYearsBeforePresent,
                IdentifiedFamily = _idService.GetFamily(record),
                IdentifiedGenus = _idService.GetGenus(record),
                IdentifiedSpecies = _idService.GetSpecies(record),
                ImageUrls = record.Images.Select(m => m.FileName).ToList(),
                Latitude = record.Latitude,
                Longitude = record.Longitude,
                TimeAdded = record.TimeAdded,
                ImageScale = record.Images.Select(m => m.ScaleNanoMetres).First(),
                Identifications = record.Identifications
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Identify(IdentificationViewModel result)
        {
            //Vadiation
            if (result.TaxonomicResolution == Taxonomy.Family && string.IsNullOrEmpty(result.Family))
                ModelState.AddModelError("Family", "You must specify a family name");
            if (result.TaxonomicResolution == Taxonomy.Genus && string.IsNullOrEmpty(result.Genus))
                ModelState.AddModelError("Genus", "You must specify a genus name");
            if (result.TaxonomicResolution == Taxonomy.Species && (string.IsNullOrEmpty(result.Species) || string.IsNullOrEmpty(result.Genus)))
                ModelState.AddModelError("Species", "You must enter both the genus and species");

            var record = _grainService.GetById(result.GrainId);
            Identification myIdentification = null;
            if (record != null)
            {
                myIdentification = _idService.GetUsersIdentification(record.GrainId, User.GetUserId());
                if (myIdentification != null) ModelState.AddModelError("User", "You have already identified this grain, sorry!");
            }

            if (ModelState.ErrorCount > 0)
            {
                //Repopulate view model
                //TODO Use a partial to avoid this
                var viewModel = new IdentificationViewModel()
                {
                    AlreadyIdentifiedByUser = myIdentification != null,
                    UserIdentification = myIdentification,
                    GrainId = record.GrainId,
                    Age = record.AgeYearsBeforePresent,
                    IdentifiedFamily = _idService.GetFamily(record),
                    IdentifiedGenus = _idService.GetGenus(record),
                    IdentifiedSpecies = _idService.GetSpecies(record),
                    ImageUrls = record.Images.Select(m => m.FileName).ToList(),
                    Latitude = record.Latitude,
                    Longitude = record.Longitude,
                    TimeAdded = record.TimeAdded,
                    ImageScale = record.Images.Select(m => m.ScaleNanoMetres).First(),
                    Identifications = record.Identifications,
                    Family = result.Family,
                    Genus = result.Genus,
                    Species = result.Species
                };
                return View(viewModel);
            }

            //Save identification
            AppUser currentUser = await UserManager.FindByIdAsync(User.GetUserId());
            var identification = new Identification()
            {
                Family = result.Family,
                Genus = result.Genus,
                Species = result.Species,
                Grain = record,
                Time = DateTime.Now,
                User = currentUser,
                Rank = result.TaxonomicResolution
            };
            _idService.SaveIdentification(identification);

            ViewData["successMessage"] = "Thank you! Your identification has been registered.";
            return RedirectToAction("Identify", new { id = result.GrainId });
        }

        [Authorize]
        public IActionResult MyGrains()
        {
            var thisUser = User.GetUserId();
            var grains = _grainService.GetByUser(thisUser).ToList();
            var model = grains.Select(m => new ReadOnlyGrainViewModel()
            {
                Id = m.GrainId,
                ImageLocation = m.Images.First().FileName,
                Bounty = BountyUtility.Calculate(m.TimeAdded),
                TimeAdded = m.TimeAdded
            }).ToList();
            return View(model);
        }

        [Authorize]
        public IActionResult RemoveIdentification(int identificationId)
        {
            //Check Prerequisites
            if (identificationId == 0) return HttpBadRequest();
            var existingId = _idService.GetById(identificationId);
            if (existingId == null) return HttpNotFound();
            if (_idService.HasConfirmedIdentity(existingId.Grain)) return HttpBadRequest();
            if (existingId.User.Id != User.GetUserId()) return HttpUnauthorized();

            //Execute
            var grainId = existingId.Grain.GrainId;
            _idService.Remove(existingId);
            return RedirectToAction("Identify", new { id = grainId });
        }

        public IActionResult Help()
        {
            return View();
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = User.GetUserId();
            var grain = _grainService.GetById(id);
            if (grain.SubmittedBy.Id != userId) return HttpBadRequest("Can only delete grains that were submitted by you");
            var deleted = _grainService.MarkDeleted(grain);
            return View("MyGrains");
        }
    }
}
