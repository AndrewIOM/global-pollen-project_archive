using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.Framework.Runtime;
using OxPollen.ViewModels;
using Microsoft.Data.Entity;
using OxPollen.Services;

namespace OxPollen.Controllers
{
    public class PollenController : Controller
    {
        private readonly PollenDbContext _context;
        private readonly IdentificationService _idService;
        private IApplicationEnvironment _hostingEnvironment;

        public PollenController(PollenDbContext context, IdentificationService idService, IApplicationEnvironment env)
        {
            _context = context;
            _idService = idService;
            _hostingEnvironment = env;
        }

        // GET: /<controller>/
        public IActionResult Index(int? id)
        {
            var model = _context.PollenRecords.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Identify(int id)
        {
            //TODO Lazy loading not implemented in EF beta 5
            var record = _context.PollenRecords.Include(c => c.Identifications).Include(c => c.Taxon).ToList()
                .FirstOrDefault(m => m.PollenRecordId == id);
            if (record == null)
            {
                //If invalid ID, redirect to index view
                return RedirectToAction("Index");
            }

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
            //Refetch data
            var record = _context.PollenRecords.FirstOrDefault(m => m.PollenRecordId == result.GrainId);
            result.Grain = record;

            if (!ModelState.IsValid)
            {
                return View(result);
            }

            var identification = new Identification();
            identification.Record = record;
            identification.TaxonName = result.TaxonName;
            identification.TimeIdentified = DateTime.Now;
            identification.UserId = User.GetUserId();

            _idService.SaveIdentification(identification);
            UpdateGrainIdentificationStatus(identification.Record.PollenRecordId);

            //TODO Stop this refetching
            var record2 = _context.PollenRecords.Include(c => c.Identifications).Include(c => c.Taxon).ToList()
                .FirstOrDefault(m => m.PollenRecordId == result.GrainId);
            result.Grain = record2;

            //TempData["successMessage"] = "Thank you! Your identification has been registered.";
            result.AlreadyIdentifiedByUser = true;
            return View(result);
        }

        [Authorize]
        public IActionResult Add()
        {
            var model = new PollenRecord();
            var userId = User.GetUserId();
            var userName = User.GetUserName();
            return View();
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
            result.Taxon = null;
            result.HasConfirmedIdentity = false;
            result.TimeAdded = DateTime.Now;

            //Handle files
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"');// FileName returns "fileName.ext"(with double quotes) in beta 3
                if (fileName.EndsWith(".jpg"))// Important for security if saving in webroot
                {
                    var guid = Guid.NewGuid();
                    var filePath = _hostingEnvironment.ApplicationBasePath + "\\wwwroot\\grain-images\\" + guid + ".jpg";
                    await file.SaveAsAsync(filePath);
                    result.PhotoUrl = "\\grain-images\\" + guid + ".jpg";
                }
            }

            _context.Add(result);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void UpdateGrainIdentificationStatus(int grainId)
        {
            //TODO Remove temp fix for lack of lazy loading in beta5
            var grain = _context.PollenRecords.Include(c => c.Identifications).ToList()
                .FirstOrDefault(m => m.PollenRecordId == grainId);
            if (grain == null) return;

            var totalIdentifications = grain.Identifications.Count;
            if (totalIdentifications < 3)
            {
                grain.HasConfirmedIdentity = false;
            }
            else
            {
                int percentAgreementRequired = 100;
                var groups = grain.Identifications.GroupBy(m => m.TaxonName);
                var percentAgreement = (groups.Count() / (percentAgreementRequired / 100)) * 100;
                grain.HasConfirmedIdentity = percentAgreement >= percentAgreementRequired ? true : false;
            }

            _context.SaveChanges();
        }

    }
}
