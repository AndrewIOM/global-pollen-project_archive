using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.WebUI.Models.Reference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GlobalPollenProject.WebUI.Extensions;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class ReferenceController : Controller
    {
        private readonly IDigitisationService _digitiseAppService;
        private readonly IUserService _userAppService;
        public ReferenceController(IDigitisationService digitiseAppService, IUserService userAppService)
        {
            _digitiseAppService = digitiseAppService;
            _userAppService = userAppService;
        }

        public IActionResult Index(int p = 1, int pageSize = 40)
        {
            var model = _digitiseAppService.GetCollections(p, pageSize).Result;
            return View(model);
        }

        public IActionResult Collection(int id)
        {
            var model = _digitiseAppService.GetCollection(id).Result;
            return View(model);
        }

        public IActionResult Grain(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var model = _digitiseAppService.GetSlide(id).Result;
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestAccess()
        {
            var model = new RequestAccessViewModel();
            model.HasRequestedAccess = _digitiseAppService.HasDigitisationRights().Result;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RequestAccess(RequestAccessViewModel result)
        {
            if (!ModelState.IsValid) return BadRequest();
            _digitiseAppService.RequestDigitisationRights();
            return RedirectToAction("RequestAccess");
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddCollection()
        {
            var model = new DigitisedCollection();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddCollection(DigitisedCollection result)
        {
            if (!ModelState.IsValid)
            {
                return View(result);
            }

            var serviceResult = _digitiseAppService.CreateCollection(result);
            if (!serviceResult.IsValid)
            {
                ModelState.AddServiceErrors(serviceResult.Messages);
                return View(result);
            }

            var collectionId = serviceResult.Result.Id;
            return RedirectToAction("Collection", new { id = collectionId });
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public async Task<IActionResult> EditCollection(int id)
        {
            var collection = _digitiseAppService.GetCollection(id).Result;
            if (collection == null) return BadRequest();
            var currentUser = await _userAppService.GetUser(User.Identity.Name);
            if (collection.UserId != currentUser.Result.Id) return Unauthorized();
            return View("AddCollection", collection);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public async Task<IActionResult> EditCollection(DigitisedCollection result)
        {
            var collection = _digitiseAppService.GetCollection(result.Id).Result;
            if (collection == null) return BadRequest();
            var currentUser = await _userAppService.GetUser(User.Identity.Name);
            if (collection.UserId != currentUser.Result.Id) return Unauthorized();

            if (!ModelState.IsValid)
            {
                return View("AddCollection", result);
            }

            var serviceResult = _digitiseAppService.UpdateCollectionMetadata(result);
            if (!serviceResult.IsValid)
            {
                ModelState.AddServiceErrors(serviceResult.Messages);
                return View(result);
            }

            return RedirectToAction("Collection", new { id = result.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public async Task<IActionResult> AddSlide(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var collection = _digitiseAppService.GetCollection(id);
            var currentUser = await _userAppService.GetUser(User.Identity.Name);
            if (collection.Result.UserId != currentUser.Result.Id) return Unauthorized();
            return View(new AddDigitisedSlide()
            {
                CollectionId = collection.Result.Id
            });
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public async Task<IActionResult> AddSlide(AddDigitisedSlide result)
        {
            var collection = _digitiseAppService.GetCollection(result.CollectionId.Value).Result;
            if (collection == null) return NotFound();

            var currentUser = await _userAppService.GetUser(User.Identity.Name);
            if (collection.UserId != currentUser.Result.Id)
            {
                ModelState.AddModelError(null, "You can only add grains to collections you own.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceResult = _digitiseAppService.AddSlide(collection.Id, result);
            if (!serviceResult.IsValid)
            {
                ModelState.AddServiceErrors(serviceResult.Messages);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize(Roles = "Digitise")]
        public IActionResult DeleteSlide(int id)
        {
            var slide = _digitiseAppService.GetSlide(id);
            if (!slide.IsValid) return BadRequest();
            //if (User.Identity.Name != slide.Result) return BadRequest();
            // Must own the collection the slide is in...

            var serviceResult = _digitiseAppService.RemoveSlide(id);
            if (!serviceResult.IsValid)
            {
                ModelState.AddServiceErrors(serviceResult.Messages);
                return BadRequest(ModelState);
            }
            return RedirectToAction("Collection", new { id = 1 }); //grain.Collection.Id }); 
            // TODO fix this
        }

    }
}
