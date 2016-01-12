using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.Services;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OxPollen.Controllers
{
    public class ReferenceController : Controller
    {
        private IFileStoreService _fileService;
        private IReferenceService _refService;
        private IUserService _userService;
        private readonly IEmailSender _emailSender;
        public ReferenceController(
            IFileStoreService fileService, 
            IReferenceService refService, 
            IUserService userService,
            IEmailSender emailSender)
        {
            _fileService = fileService;
            _refService = refService;
            _userService = userService;
            _emailSender = emailSender;
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

        public IActionResult Index()
        {
            return View(new RequestAccessViewModel());
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
            var adminEmail = _userService.GetAll().First().Email; //Temporary hack
            _emailSender.SendEmailAsync(adminEmail, "Request for digitisation rights",
                user.FullName() + " has requested digitisation rights. They write: " + result.Comments).Wait();
            return View("Index");
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
            return RedirectToAction("Collections", new { id = saved.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(int id)
        {
            if (id == 0)
            {
                var currentUser = User.GetUserId();
                var listModel = _refService.GetCollectionsByUser(currentUser);
                return View("Collections", listModel);
            }
            var model = _refService.GetCollectionById(id);
            if (model.User.Id != User.GetUserId()) return HttpBadRequest();
            return View("AddGrains", model);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(ReferenceGrainViewModel result)
        {
            //TODO Limit addition to current user's collections only

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
            return Ok(saved);
        }

        [Authorize(Roles = "Digitise")]
        public IActionResult Collections(int id)
        {
            if (id == 0)
            {
                var currentUser = User.GetUserId();
                var listModel = _refService.GetCollectionsByUser(currentUser);
                return View("Collections", listModel);
            }
            var model = _refService.GetCollectionById(id);
            if (model.User.Id != User.GetUserId()) return HttpBadRequest();
            return View("CollectionDetail", model);
        }
    }
}
