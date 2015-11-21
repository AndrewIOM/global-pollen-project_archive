using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Data.Entity;
using OxPollen.Models;
using OxPollen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using OxPollen.Services.Abstract;

namespace OxPollen.Controllers
{
    public class GrainController : Controller
    {
        private IGrainService _grainService;
        private IIdentificationService _idService;
        private IFileStoreService _uploadService;
        public GrainController(IIdentificationService id, IGrainService grain, IFileStoreService fileService)
        {
            _idService = id;
            _grainService = grain;
            _uploadService = fileService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            //TODO Ordering by user
            var model = _grainService.GetUnidentifiedGrains()
                .OrderByDescending(m => m.Bounty).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            var record = _grainService.GetById(id);
            if (record == null) return RedirectToAction("Index");

            bool identifiedByMe = false;
            var identifications = record.Identifications.Select(m => m.UserId);
            if (User.IsSignedIn())
            {
                identifiedByMe = (identifications.Contains(User.GetUserId())) ? true : false;
            }

            var viewModel = new IdentificationViewModel()
            {
                AlreadyIdentifiedByUser = identifiedByMe,
                Grain = record,
                GrainId = record.PollenRecordId
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Identify(IdentificationViewModel result)
        {
            var record = _grainService.GetById(result.GrainId);
            result.Grain = record;

            if (!ModelState.IsValid)
            {
                return View(result);
            }

            var userId = User.GetUserId();
            if (record.Identifications.Select(m => m.UserId).Contains(userId))
            {
                return RedirectToAction("Identify", new { id = result.GrainId });
            }

            var identification = new Identification();
            identification.Record = record;
            identification.TaxonName = result.TaxonName;
            identification.TimeIdentified = DateTime.Now;
            identification.UserId = User.GetUserId();

            _idService.SaveIdentification(identification);

            ViewData["successMessage"] = "Thank you! Your identification has been registered.";
            result.Grain = _grainService.GetById(result.GrainId);
            result.AlreadyIdentifiedByUser = true;
            return View(result);
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new PollenRecord();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(PollenRecord result, IList<IFormFile> files)
        {
            if (!ModelState.IsValid || files.Count == 0)
            {
                if (files.Count == 0) ModelState.AddModelError("PhotoUrl", "You must upload an image of your pollen grain");
                return View(result);
            }

            result.UserId = User.GetUserId();
            result.TimeAdded = DateTime.Now;
            var uploadedFile = await _uploadService.Upload(files.First());
            result.PhotoUrl = uploadedFile;
            _grainService.Add(result);
            return RedirectToAction("Index");
        }

        public IActionResult Help()
        {
            return View();
        }
    }
}
