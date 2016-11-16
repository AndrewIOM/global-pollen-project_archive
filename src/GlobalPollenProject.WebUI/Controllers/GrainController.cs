using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.WebUI.Extensions;
using GlobalPollenProject.WebUI.Models;
using GlobalPollenProject.WebUI.Models.Grain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class GrainController : Controller
    {
        private IIdentificationService _appService;
        public GrainController(IIdentificationService appService)
        {
            _appService = appService;
        }

        public IActionResult Index(int pageSize = 20, int page = 1, GrainSearchFilter filter = null)
        {
            if (filter == null) filter = new GrainSearchFilter();
            var grains = _appService.GetUnknownGrains(filter, pageSize, page);

            var model = new FilteredGrainsViewModel()
            {
                Filters = filter,
                Grains = grains.Result
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new AddUnknownGrain();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddUnknownGrain result)
        {
            if (!string.IsNullOrEmpty(result.ImageOne)) if (!IsBase64String(result.ImageOne)) ModelState.AddModelError("ImageOne", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageTwo)) if (!IsBase64String(result.ImageTwo)) ModelState.AddModelError("ImageTwo", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageThree)) if (!IsBase64String(result.ImageThree)) ModelState.AddModelError("ImageThree", "Image not ecoded in base64");
            if (!string.IsNullOrEmpty(result.ImageFour)) if (!IsBase64String(result.ImageFour)) ModelState.AddModelError("ImageFour", "Image not ecoded in base64");
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = await _appService.UploadUnknownGrain(result);
            if (!serviceResult.IsValid)
            {
                ModelState.AddServiceErrors(serviceResult.Messages);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            var result = _appService.GetUnknownGrain(id);
            if (!result.IsValid)
            {
                ModelState.AddServiceErrors(result.Messages);
                return NotFound();
                //return RedirectToAction("Index");
            }

            var previousId = result.Result.Status.Identifications.FirstOrDefault(m => m.SubmittedBy == User.Identity.Name);
            var viewModel = new IdentificationFormViewModel() 
            {
                Grain = result.Result,
                UserIdentification = previousId
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Identify(IdentificationFormViewModel result)
        {
            var grainResult = _appService.GetUnknownGrain(result.Grain.Id);
            if (!grainResult.IsValid)
            {
                ModelState.AddServiceErrors(grainResult.Messages);
                return RedirectToAction("Index");
            }
            result.Grain = grainResult.Result;

            var idResult = await _appService.IdentifyAs(result.Grain.Id, result.Family, result.Genus, result.Species);
            if (!idResult.IsValid)
            {
                ModelState.AddServiceErrors(idResult.Messages);
                return View(result);
            }

            ViewData["Success"] = "Thank you! Your identification has been registered.";
            return RedirectToAction("Identify", new { id = grainResult.Result.Id });
        }

        [Authorize]
        public async Task<IActionResult> RemoveIdentification(int grainId)
        {
            var result = await _appService.RemoveIdentification(grainId);
            if (!result.IsValid)
            {
                ModelState.AddServiceErrors(result.Messages);
                return BadRequest(ModelState);
            }
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
