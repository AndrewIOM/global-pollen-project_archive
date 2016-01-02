using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OxPollen.Controllers
{
    [Authorize]
    public class ReferenceController : Controller
    {
        private IFileStoreService _fileService;
        private IReferenceService _refService;
        private IUserService _userService;
        public ReferenceController(IFileStoreService fileService, IReferenceService refService, IUserService userService)
        {
            _fileService = fileService;
            _refService = refService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult AddCollection()
        {
            var model = new ReferenceCollection();
            return View(model);
        }

        [HttpPost]
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

        [HttpPost]
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
                    FileName = file,
                    ScaleNanoMetres = result.ImagesScale.Value
                });
            }

            var saved = _refService.AddGrain(toSave);
            return Ok(saved);
        }

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

        public IActionResult Grain(int id)
        {
            if (id == 0)
            {
                return HttpBadRequest();
            }
            var model = _refService.GetGrainById(id);
            return View(model);
        }
    }
}
