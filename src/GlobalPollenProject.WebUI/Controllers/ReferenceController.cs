using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.WebUI.Models.Reference;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class ReferenceController : Controller
    {
        private readonly IDigitisationService _digitiseAppService;
        public UserManager<AppUser> UserManager { get; set; }
        public ReferenceController(IDigitisationService digitiseAppService, IServiceProvider services)
        {
            _digitiseAppService = digitiseAppService;
            UserManager = services.GetRequiredService<UserManager<AppUser>>();
        }

        public IActionResult Index()
        {
            var model = _digitiseAppService.GetCollections();
            return View(model);
        }

        public IActionResult Collection(int id)
        {
            var model = _digitiseAppService.GetCollection(id);
            return View(model);
        }

        public IActionResult Grain(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var model = _digitiseAppService.GetSlide(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult RequestAccess()
        {
            var model = new RequestAccessViewModel();
            model.HasRequestedAccess = _digitiseAppService.HasDigitisationRights();
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
            if (!ModelState.IsValid) return BadRequest();
            _digitiseAppService.RequestDigitisationRights();
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
        public IActionResult AddCollection(DigitisedCollection result)
        {
            if (!ModelState.IsValid)
            {
                return View(result);
            }

            _digitiseAppService.CreateCollection(result);
            //return RedirectToAction("Collection", new { id = saved.Id });
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult EditCollection(int id)
        {
            var collection = _digitiseAppService.GetCollection(id);
            if (collection == null)
            {
                return BadRequest();
            }
            // if (collection.User.Id != UserManager.GetUserId(User))
            // {
            //     return Unauthorized();
            // }

            return View("AddCollection", collection);
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public IActionResult EditCollection(DigitisedCollection result)
        {
            // var collection = _digitiseAppService.GetCollection(result.Id);
            // if (collection == null)
            // {
            //     return BadRequest();
            // }
            // if (collection.User.Id != UserManager.GetUserId(User))
            // {
            //     return Unauthorized();
            // }

            if (!ModelState.IsValid)
            {
                return View("AddCollection", result);
            }

            // collection.CountryCode = model.CountryCode;
            // collection.Description = model.Description;
            // collection.FocusRegion = model.FocusRegion;
            // collection.Institution = model.Institution;
            // collection.OwnedBy = model.OwnedBy;
            // collection.Name = model.Name;
            // collection.WebAddress = model.WebAddress;
            // collection.ContactEmail = model.ContactEmail;
            _digitiseAppService.UpdateCollectionMetadata(result);
            //return RedirectToAction("Collection", new { id = result.Id });
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles = "Digitise")]
        public IActionResult AddGrain(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var collection = _digitiseAppService.GetCollection(id);
            // if (model.User.Id != UserManager.GetUserId(User)) return BadRequest();
            return View(new AddDigitisedSlide()
            {
                //CollectionId = model.Id
            });
        }

        [HttpPost]
        [Authorize(Roles = "Digitise")]
        public async Task<IActionResult> AddGrain(AddDigitisedSlide result)
        {
            var collection = _digitiseAppService.GetCollection(result.CollectionId.Value);
            if (collection == null)
            {
                ModelState.AddModelError("CollectionId", "The collection specified does not exist.");
            } else
            {
                // if (collection.User.Id != UserManager.GetUserId(User))
                // {
                //     ModelState.AddModelError("CollectionId", "You can only add grains to collections you own.");
                // }
            }

            // if (!_backbone.IsValidTaxon(result.Rank, result.Family, result.Genus, result.Species))
            // {
            //     ModelState.AddModelError("TaxonomicBackbone", "The taxon specified was not matched by our taxonomic backbone. Check your spellings and try again");
            // }

            foreach (var image in result.Images)
            {
                if (!string.IsNullOrEmpty(image)) if (!IsBase64String(image)) ModelState.AddModelError("Images", "There was an encoding error when uploading your image. Please try a different image, or report the problem.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //_digitiseAppService.AddSlide(collection.Id, result);

            return Ok();
        }

        [Authorize(Roles = "Digitise")]
        public IActionResult DeleteGrain(int id)
        {
            var slide = new DigitisedSlide();
            _digitiseAppService.RemoveSlide(slide);
            // var grain = _refService.GetGrainById(id);
            // if (grain == null) return BadRequest();
            // if (User.Identity.Name != grain.Collection.User.UserName) return BadRequest();
            // _refService.DeleteGrain(id);
            // return RedirectToAction("Collection", new { id = grain.Collection.Id });
            throw new NotImplementedException();
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

        private string GetName(Taxonomy rank, ReferenceGrain grain)
        {
            string species = null;
            string genus = null;
            string family = null;
            if (grain.Taxon != null)
            {
                if (grain.Taxon.Rank == Taxonomy.Species)
                {
                    species = grain.Taxon.LatinName;
                    genus = grain.Taxon.ParentTaxa.LatinName;
                    family = grain.Taxon.ParentTaxa.ParentTaxa.LatinName;
                }
                else if (grain.Taxon.Rank == Taxonomy.Genus)
                {
                    genus = grain.Taxon.LatinName;
                    family = grain.Taxon.ParentTaxa.LatinName;
                }
                else
                {
                    family = grain.Taxon.LatinName;
                }
            }
            if (rank == Taxonomy.Species) return species;
            if (rank == Taxonomy.Genus) return genus;
            return family;
        }

    }
}
