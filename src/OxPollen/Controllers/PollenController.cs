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

namespace OxPollen.Controllers
{
    public class PollenController : Controller
    {
        private readonly PollenDbContext _context;
        private IApplicationEnvironment _hostingEnvironment;
        public PollenController(PollenDbContext context, IApplicationEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: /<controller>/
        public IActionResult Index(int? id)
        {
            var model = _context.PollenRecords.ToList();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var result = _context.PollenRecords.FirstOrDefault(m => m.PollenRecordId == id);
            if (result == null) return View(_context.PollenRecords.ToList());
            return View(result);
        }

        [Authorize]
        public IActionResult Identify(Identification result)
        {
            if (!ModelState.IsValid)
            {
                return View(result);
            }

            

            return View(result)
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
    }
}
