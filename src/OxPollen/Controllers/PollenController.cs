using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace OxPollen.Controllers
{
    public class PollenController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
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
        public IActionResult Add(PollenRecord result)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            throw new NotImplementedException();
        }
    }
}
