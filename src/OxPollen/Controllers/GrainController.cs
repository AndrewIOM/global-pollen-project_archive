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
            var model = new AddGrainViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddGrainViewModel result, IList<IFormFile> files)
        {
            //Validation
            if (files.Count == 0) ModelState.AddModelError("image", "You must upload an image");
            foreach (var file in files)
            {
                if (!FileUploadUtility.IsImage(file)) ModelState.AddModelError("image", "files must be images");
            }
            if (ModelState.ErrorCount > 0)
            {
                return View(result);
            }

            //Populate new Grain
            AppUser currentUser = await UserManager.FindByNameAsync(User.GetUserName());
            var uploadedFiles = await _uploadService.Upload(files);
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
                    ScaleNanoMetres = 1 //TODO remove hardcoding
                });
            }
            _grainService.Add(grain);
            return RedirectToAction("Index");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var grains = _grainService.GetUnidentifiedGrains();
            var model = grains
                .OrderByDescending(m => BountyUtility.Calculate(m.TimeAdded)).Select(m => new GrainViewModel()
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

            bool identifiedByMe = false;
            if (User.IsSignedIn())
            {
                identifiedByMe = _idService.IsIdentifiedByUser(id, User.GetUserId());
            }

            var viewModel = new IdentificationViewModel()
            {
                AlreadyIdentifiedByUser = identifiedByMe,
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
            bool alreadyIdentified = false;
            if (record != null)
            {
                alreadyIdentified = _idService.IsIdentifiedByUser(record.GrainId, User.GetUserId());
                if (alreadyIdentified) ModelState.AddModelError("User", "You have already identified this grain, sorry!");
            }

            if (ModelState.ErrorCount > 0)
            {
                //Repopulate view model
                //TODO Use a partial to avoid this
                var viewModel = new IdentificationViewModel()
                {
                    AlreadyIdentifiedByUser = alreadyIdentified,
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

        public IActionResult Help()
        {
            return View();
        }
    }
}
