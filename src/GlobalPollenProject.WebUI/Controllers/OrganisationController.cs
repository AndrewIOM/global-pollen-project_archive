using System;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class OrganisationController : Controller
    {
        private readonly IUserService _userService;
        public OrganisationController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult View(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddClub result)
        {
            if (ModelState.IsValid)
            {
                return View(result);
            }

            var appResult = _userService.CreateClub(result);
            if (!appResult.IsValid)
            {
                ModelState.AddServiceErrors(appResult.Messages);
                return View(result);
            }

            return RedirectToAction("View", new {id = appResult.Result.Id });
        }


        public IActionResult GetAll()
        {
            // var result = _context.Organisations.Select(m => new OrganisationViewModel()
            // {
            //     Id = m.OrganisationId,
            //     Name = m.Name
            // }).ToList();
            // return Ok(result);
            return Ok();
        }

        public IActionResult Search(string searchTerm)
        {
            // var result = _context.Organisations.Where(m => m.Name.Contains(searchTerm))
            //     .Select(m => new OrganisationViewModel()
            // {
            //     Id = m.OrganisationId,
            //     Name = m.Name
            // }).ToList();
            // return Ok(result);
            return Ok();
        }
    }
}
