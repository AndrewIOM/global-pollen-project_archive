using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Models.Grain;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class GrainController : Controller
    {
        private IIdentificationService _appService;
        public GrainController(IIdentificationService appService)
        {
            _appService = appService;
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new UnknownGrain();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddUnknownGrain result)
        {
            if (!string.IsNullOrEmpty(result.ImageOne)) if (!IsBase64String(result.ImageOne)) ModelState.AddModelError("ImageOne", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageTwo)) if (!IsBase64String(result.ImageTwo)) ModelState.AddModelError("ImageTwo", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageThree)) if (!IsBase64String(result.ImageThree)) ModelState.AddModelError("ImageThree", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageFour)) if (!IsBase64String(result.ImageFour)) ModelState.AddModelError("ImageFour", "Image not ecoded in base64");
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            _appService.UploadUnknownGrain(result);

            return Ok();
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            var grain = _appService.GetUnknownGrain(id);
            if (grain == null) return RedirectToAction("Index");

            var viewModel = new IdentificationFormViewModel() 
            {
                Grain = grain
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Identify(IdentificationFormViewModel result)
        {
            var grain = _appService.GetUnknownGrain(result.Grain.Id);
            // Identification myIdentification = null;
            // if (record != null)
            // {
            //     myIdentification = _idService.GetByUser(UserManager.GetUserId(User))
            //         .FirstOrDefault(m => m.Grain.Id == result.GrainId);
            //     if (myIdentification != null) ModelState.AddModelError("User", "You have already identified this grain, sorry!");
            // }
            // if (!_taxonomy.IsValidTaxon(result.TaxonomicResolution, result.Family, result.Genus, result.Species))
            // {
            //     ModelState.AddModelError("Family", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            // }

            if (ModelState.ErrorCount > 0)
            {
                var viewModel = new IdentificationFormViewModel() 
                {
                    Grain = grain
                };
                return View(viewModel);
            }

            ViewData["Success"] = "Thank you! Your identification has been registered.";
            return RedirectToAction("Identify", new { id = grain.Id });
        }

        public IActionResult Index(GrainSearchFilter filter = null)
        {
            if (filter == null) filter = new GrainSearchFilter();
            var grains = _appService.GetUnknownGrains(filter);

            var model = new FilteredGrainsViewModel()
            {
                Filters = filter,
                Grains = grains
            };

            return View(model);
        }

        [Authorize]
        public IActionResult MyGrains()
        {
            var grains = _appService.GetMyUnknownGrains();
            return View(grains);
        }

        [Authorize]
        public IActionResult RemoveIdentification(int grainId)
        {
            //Check Prerequisites
            // if (existingId == null) return NotFound();
            // //TODO Stop removal if identity confirmed: if (existingId.Grain.) return BadRequest();
            // if (existingId.User.Id != UserManager.GetUserId(User)) return Unauthorized();
            
            // var grainId = existingId.Grain.Id;
            _appService.RemoveIdentification(grainId);
            return RedirectToAction("Identify", new { id = grainId });
        }

        // [Authorize]
        // public IActionResult Delete(int id)
        // {
        //     var userId = UserManager.GetUserId(User);
        //     var grain = _grainService.GetById(id);
        //     // if (_idService.HasConfirmedIdentity(grain)) return BadRequest("Can't delete grains with confirmed identity");

        //     if (User.IsInRole("Admin") || grain.SubmittedBy.Id != userId)
        //     {
        //         var deleted = _grainService.MarkDeleted(grain.Id);
        //         return RedirectToAction("MyGrains");
        //     }
        //     return BadRequest("Can only delete grains that were submitted by you");
        // }

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

    }
}
