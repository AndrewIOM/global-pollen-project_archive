using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OxPollen.Models;
using OxPollen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using OxPollen.Services.Abstract;
using OxPollen.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using OxPollen.ViewModels.Grain;

namespace OxPollen.Controllers
{
    public class GrainController : Controller
    {
        private IGrainService _grainService;
        private IIdentificationService _idService;
        private IFileStoreService _uploadService;
        private readonly ITaxonomyBackbone _taxonomy;
        public UserManager<AppUser> UserManager { get; set; }
        public GrainController(IIdentificationService id, IGrainService grain,
            IServiceProvider services, IFileStoreService fileService, ITaxonomyBackbone backbone)
        {
            _idService = id;
            _grainService = grain;
            _uploadService = fileService;
            UserManager = services.GetRequiredService<UserManager<AppUser>>();
            _taxonomy = backbone;
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new GrainViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(GrainViewModel result)
        {
            if (!string.IsNullOrEmpty(result.ImageOne)) if (!IsBase64String(result.ImageOne)) ModelState.AddModelError("ImageOne", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageTwo)) if (!IsBase64String(result.ImageTwo)) ModelState.AddModelError("ImageTwo", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageThree)) if (!IsBase64String(result.ImageThree)) ModelState.AddModelError("ImageThree", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageFour)) if (!IsBase64String(result.ImageFour)) ModelState.AddModelError("ImageFour", "Image not ecoded in base64");
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            //Populate new Grain
            var filesToUpload = new List<string>();
            filesToUpload.Add(result.ImageOne);
            if (!string.IsNullOrEmpty(result.ImageTwo)) filesToUpload.Add(result.ImageTwo);
            if (!string.IsNullOrEmpty(result.ImageThree)) filesToUpload.Add(result.ImageThree);
            if (!string.IsNullOrEmpty(result.ImageFour)) filesToUpload.Add(result.ImageFour);
            var uploadedFiles = _uploadService.UploadBase64Image(filesToUpload);

            AppUser currentUser = await UserManager.FindByNameAsync(UserManager.GetUserName(User));
            var grain = new Grain()
            {
                AgeYearsBeforePresent = result.AgeYearsBeforePresent,
                Latitude = result.Latitude.Value,
                Longitude = result.Longitude.Value,
                SubmittedBy = currentUser,
                MaxSizeNanoMetres = result.ImagesScale.Value,
                TimeAdded = DateTime.Now,
                Images = new List<GrainImage>()
            };
            foreach (var file in uploadedFiles)
            {
                grain.Images.Add(new GrainImage()
                {
                    FileName = file.Url,
                    FileNameThumbnail = file.ThumbnailUrl
                });
            }
            _grainService.Add(grain);
            return Ok();
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            var record = _grainService.GetById(id);
            if (record == null) return RedirectToAction("Index");

            Identification myIdentification = null;
            if (User.Identity.IsAuthenticated)
            {
                myIdentification = _idService.GetByUser(UserManager.GetUserId(User))
                    .FirstOrDefault(m => m.Grain.Id == id);
            }

            var viewModel = new IdentificationViewModel()
            {
                AlreadyIdentifiedByUser = myIdentification != null,
                UserIdentification = myIdentification,
                GrainId = record.Id,
                Age = record.AgeYearsBeforePresent,
                IdentifiedFamily = GetIdentifiedName(Taxonomy.Family, record),
                IdentifiedGenus = GetIdentifiedName(Taxonomy.Genus, record),
                IdentifiedSpecies = GetIdentifiedName(Taxonomy.Species, record),
                ImageUrls = record.Images.Select(m => m.FileName).ToList(),
                Latitude = record.Latitude,
                Longitude = record.Longitude,
                TimeAdded = record.TimeAdded,
                ImageScale = record.MaxSizeNanoMetres,
                Identifications = record.Identifications
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Identify(IdentificationViewModel result)
        {
            var record = _grainService.GetById(result.GrainId);
            Identification myIdentification = null;
            if (record != null)
            {
                myIdentification = _idService.GetByUser(UserManager.GetUserId(User))
                    .FirstOrDefault(m => m.Grain.Id == result.GrainId);
                if (myIdentification != null) ModelState.AddModelError("User", "You have already identified this grain, sorry!");
            }

            if (!_taxonomy.IsValidTaxon(result.TaxonomicResolution, result.Family, result.Genus, result.Species))
            {
                ModelState.AddModelError("Family", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            }

            if (ModelState.ErrorCount > 0)
            {
                //Repopulate view model
                //TODO Use a partial to avoid this
                var viewModel = new IdentificationViewModel()
                {
                    AlreadyIdentifiedByUser = myIdentification != null,
                    UserIdentification = myIdentification,
                    GrainId = record.Id,
                    Age = record.AgeYearsBeforePresent,
                    IdentifiedFamily = GetIdentifiedName(Taxonomy.Family, record),
                    IdentifiedGenus = GetIdentifiedName(Taxonomy.Genus, record),
                    IdentifiedSpecies = GetIdentifiedName(Taxonomy.Species, record),
                    ImageUrls = record.Images.Select(m => m.FileName).ToList(),
                    Latitude = record.Latitude,
                    Longitude = record.Longitude,
                    TimeAdded = record.TimeAdded,
                    ImageScale = record.MaxSizeNanoMetres,
                    Identifications = record.Identifications,
                    Family = result.Family,
                    Genus = result.Genus,
                    Species = result.Species
                };
                return View(viewModel);
            }

            //Save identification
            AppUser currentUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
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
            _idService.Add(identification);

            ViewData["Success"] = "Thank you! Your identification has been registered.";
            return RedirectToAction("Identify", new { id = result.GrainId });
        }

        // GET: /<controller>/
        public IActionResult Index(GrainSearchFilter filter = null)
        {
            if (filter == null) filter = new GrainSearchFilter();
            var grains = _grainService.Search(filter);
            var simpleGrains = grains.Select(m => new SimpleGrainViewModel()
            {
                Bounty = BountyUtility.Calculate(m),
                Id = m.Id,
                TimeAdded = m.TimeAdded,
                ImageLocation = m.Images.Count > 0 ? m.Images.First().FileName : null,
                ThumbnailLocation = m.Images.Count > 0 ? m.Images.First().FileNameThumbnail : null,
                Latitude = m.Latitude,
                Longitude = m.Longitude
            }).ToList();

            var model = new FilteredGrainsViewModel()
            {
                Filters = filter,
                Grains = simpleGrains
            };

            return View(model);
        }

        [Authorize]
        public IActionResult MyGrains()
        {
            var thisUser = UserManager.GetUserId(User);
            var grains = _grainService.GetByUser(thisUser).ToList();
            var model = grains.Select(m => new SimpleGrainViewModel()
            {
                Id = m.Id,
                ImageLocation = m.Images.First().FileName,
                Bounty = BountyUtility.Calculate(m),
                TimeAdded = m.TimeAdded,
                ConfirmedFamily = GetIdentifiedName(Taxonomy.Family, m),
                ConfirmedGenus = GetIdentifiedName(Taxonomy.Genus, m),
                ConfirmedSpecies = GetIdentifiedName(Taxonomy.Species, m),
            }).ToList();
            return View(model);
        }

        [Authorize]
        public IActionResult RemoveIdentification(int identificationId)
        {
            //Check Prerequisites
            if (identificationId == 0) return BadRequest();
            var existingId = _idService.GetById(identificationId);
            if (existingId == null) return NotFound();
            //TODO Stop removal if identity confirmed: if (existingId.Grain.) return BadRequest();
            if (existingId.User.Id != UserManager.GetUserId(User)) return Unauthorized();

            //Execute
            var grainId = existingId.Grain.Id;
            _idService.Remove(existingId);
            return RedirectToAction("Identify", new { id = grainId });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = UserManager.GetUserId(User);
            var grain = _grainService.GetById(id);
            // if (_idService.HasConfirmedIdentity(grain)) return BadRequest("Can't delete grains with confirmed identity");

            if (User.IsInRole("Admin") || grain.SubmittedBy.Id != userId)
            {
                var deleted = _grainService.MarkDeleted(grain.Id);
                return RedirectToAction("MyGrains");
            }
            return BadRequest("Can only delete grains that were submitted by you");
        }

        private bool IsBase64String(string s)
        {
            try
            {
                byte[] data = Convert.FromBase64String(s);
                return (s.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                return false;
            }
        }

        private string GetIdentifiedName(Taxonomy rank, Grain record)
        {
            string species = null;
            string genus = null;
            string family = null;
            if (record.IdentifiedAs != null)
            {
                if (record.IdentifiedAs.Rank == Taxonomy.Species)
                {
                    species = record.IdentifiedAs.LatinName;
                    genus = record.IdentifiedAs.ParentTaxa.LatinName;
                    family = record.IdentifiedAs.ParentTaxa.ParentTaxa.LatinName;
                }
                else if (record.IdentifiedAs.Rank == Taxonomy.Genus)
                {
                    genus = record.IdentifiedAs.LatinName;
                    family = record.IdentifiedAs.ParentTaxa.LatinName;
                }
                else
                {
                    family = record.IdentifiedAs.LatinName;
                }
            }
            if (rank == Taxonomy.Species) return species;
            if (rank == Taxonomy.Genus) return genus;
            return family;
        }

    }
}
