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
            //TODO Limit addition to current user's collections only
            var model = _refService.GetCollectionById(id);
            if (model.User.Id != User.GetUserId()) return HttpBadRequest();
            return View("AddGrains", model);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(ReferenceGrainViewModel result)
        {
            //TODO Limit addition to current user's collections only

            Taxonomy rank;
            if (result.Species != null) { rank = Taxonomy.Species; }
            else if (result.Genus != null) { rank = Taxonomy.Genus; }
            else rank = Taxonomy.Family;
            if (!_backbone.IsValidTaxon(rank, result.Family, result.Genus, result.Species))
            {
                ModelState.AddModelError("TaxonomicBackbone", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            var filesToUpload = new List<string>();
            filesToUpload.Add(result.ImageOne);
            if (!string.IsNullOrEmpty(result.ImageTwo)) filesToUpload.Add(result.ImageTwo);
            if (!string.IsNullOrEmpty(result.ImageThree)) filesToUpload.Add(result.ImageThree);
            if (!string.IsNullOrEmpty(result.ImageFour)) filesToUpload.Add(result.ImageFour);
            var uploadedFiles = _fileService.Upload(filesToUpload);

            var toSave = new ReferenceGrain()
            {
                Collection = _refService.GetCollectionById(result.CollectionId),
                Family = result.Family,
                Genus = result.Genus,
                Species = result.Species,
                SubmittedBy = _userService.GetById(User.GetUserId()),
                TimeAdded = DateTime.Now,
                Images = new List<GrainImage>()
            };

            foreach (var file in uploadedFiles)
            {
                toSave.Images.Add(new GrainImage()
                {
                    FileName = file.Item1,
                    FileNameThumbnail = file.Item2
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
