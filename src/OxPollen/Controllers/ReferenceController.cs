using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.Services;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Reference;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OxPollen.Controllers
{
    public class ReferenceController : Controller
    {
        private IFileStoreService _fileService;
        private IReferenceService _refService;
        private IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ITaxonomyBackbone _backbone;
        public ReferenceController(
            IFileStoreService fileService,
            IReferenceService refService,
            IUserService userService,
            IEmailSender emailSender,
            ITaxonomyBackbone backbone)
        {
            _fileService = fileService;
            _refService = refService;
            _userService = userService;
            _emailSender = emailSender;
            _backbone = backbone;
        }

        public IActionResult Index()
        {
            var model = _refService.ListCollections();
            return View(model);
        }

        public IActionResult Collection(int id)
        {
            var model = _refService.GetCollectionById(id);
            return View(model);
        }

        public IActionResult Grain(int id)
        {
            if (id == 0)
            {
                return HttpBadRequest();
            }
            var model = _refService.GetGrainById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestAccess()
        {
            var model = new RequestAccessViewModel();
            var user = _userService.GetById(User.GetUserId());
            if (user != null)
            {
                model.HasRequestedAccess = user.RequestedDigitisationRights;
            }
            return View(model);
        }

        public IActionResult Help()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult RequestAccess(RequestAccessViewModel result)
        {
            if (!ModelState.IsValid) return HttpBadRequest();
            var user = _userService.GetById(User.GetUserId());
            user.RequestedDigitisationRights = true;
            _userService.Update(user);

            //Send email to all admins to let them know
            var adminEmail = "oxpollen@gmail.com"; //temporary hack
            _emailSender.SendEmailAsync(adminEmail, "Request for digitisation rights",
                user.FullName() + " has requested digitisation rights. They write: " + result.Comments).Wait();
            return RedirectToAction("RequestAccess");
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddCollection()
        {
            var model = new ReferenceCollection();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddCollection(ReferenceCollection result)
        {
            if (!ModelState.IsValid)
            {
                return View(result);
            }

            result.User = _userService.GetById(User.GetUserId());
            var saved = _refService.AddCollection(result);
            return RedirectToAction("Collection", new { id = saved.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(int id)
        {
            if (id == 0)
            {
                return HttpBadRequest();
            }
            var model = _refService.GetCollectionById(id);
            if (model.User.Id != User.GetUserId()) return HttpBadRequest();
            return View(new ReferenceGrainViewModel()
            {
                CollectionId = model.Id
            });
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult BatchAddGrains(int id)
        {
            if (id == 0)
            {
                return HttpBadRequest();
            }
            var model = _refService.GetCollectionById(id);
            if (model.User.Id != User.GetUserId()) return HttpBadRequest();
            return View("AddGrains", model);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(ReferenceGrainViewModel result)
        {
            var collection = _refService.GetCollectionById(result.CollectionId.Value);
            if (collection == null)
            {
                ModelState.AddModelError("CollectionId", "The collection specified does not exist.");
            } else
            {
                if (collection.User.Id != User.GetUserId())
                {
                    ModelState.AddModelError("CollectionId", "You can only add grains to collections you own.");
                }
            }

            if (!_backbone.IsValidTaxon(result.Rank, result.Family, result.Genus, result.Species))
            {
                ModelState.AddModelError("TaxonomicBackbone", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            }

            foreach (var image in result.Images)
            {
                if (!string.IsNullOrEmpty(image)) if (!IsBase64String(image)) ModelState.AddModelError("Images", "There was an encoding error when uploading your image. Please try a different image, or report the problem.");
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            var standardImages = _fileService.UploadBase64Image(result.Images);
            var toSave = new ReferenceGrain()
            {
                Collection = collection,
                Family = result.Family,
                Genus = result.Genus,
                Species = result.Species,
                SubmittedBy = _userService.GetById(User.GetUserId()),
                TimeAdded = DateTime.Now,
                MaxSizeNanoMetres = result.MaxGrainSize.Value,
                Images = new List<GrainImage>()
            };

            foreach (var file in standardImages)
            {
                toSave.Images.Add(new GrainImage()
                {
                    FileName = file.Url,
                    FileNameThumbnail = file.ThumbnailUrl,
                    IsFocusImage = false
                });
            }

            foreach (var image in result.FocusImages)
            {
                var low = _fileService.UploadBase64Image(image.FocusLowUrl);
                var medLow = _fileService.UploadBase64Image(image.FocusMedLowUrl);
                var med = _fileService.UploadBase64Image(image.FocusMedUrl);
                var medHigh = _fileService.UploadBase64Image(image.FocusMedHighUrl);
                var high = _fileService.UploadBase64Image(image.FocusHighUrl);
                toSave.Images.Add(new GrainImage()
                {
                    FileName = med.Url,
                    FileNameThumbnail = med.ThumbnailUrl,
                    IsFocusImage = true,
                    FocusLowUrl = low.Url,
                    FocusMedLowUrl = medLow.Url,
                    FocusMedUrl = med.Url,
                    FocusMedHighUrl = medHigh.Url,
                    FocusHighUrl = high.Url
                });
            }

            var saved = _refService.AddGrain(toSave);
            var model = new ReferenceGrainViewModel()
            {
                Family = saved.Family,
                Genus = saved.Genus,
                Species = saved.Species
            };
            return Ok(model);
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

        [Authorize(Roles = "Digitise")]
        public IActionResult DeleteCollection(int id)
        {
            var collection = _refService.GetCollectionById(id);
            if (collection == null) return HttpBadRequest();
            if (User.Identity.Name != collection.User.UserName) return HttpBadRequest();
            _refService.DeleteCollection(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Digitise")]
        public IActionResult DeleteGrain(int id)
        {
            var grain = _refService.GetGrainById(id);
            if (grain == null) return HttpBadRequest();
            if (User.Identity.Name != grain.Collection.User.UserName) return HttpBadRequest();
            _refService.DeleteGrain(id);
            return RedirectToAction("Collection", new { id = grain.Collection.Id });
        }
    }
}
