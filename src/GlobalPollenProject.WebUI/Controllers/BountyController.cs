﻿using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class BountyController : Controller
    {

        public BountyController()
        {
        }

        // GET: /<controller>/
        // public IActionResult Index()
        // {
        //     var users = _userService.GetAll();
        //     var orgs = _userService.GetOrganisations();

        //     var topOrgs = orgs.Select(m => new BountyViewModel()
        //     {
        //         Bounty = m.Members.Select(n => n.BountyScore).Sum(),
        //         Name = m.Name
        //     }).Where(m => m.Bounty > 0).OrderByDescending(m => m.Bounty).Take(10);

        //     var topUsers = users.Select(m => new BountyViewModel()
        //     {
        //         Bounty = m.BountyScore,
        //         Name = m.FirstName.Substring(0, 1) + ". " + m.LastName
        //     }).Where(m => m.Bounty > 0).OrderByDescending(m => m.Bounty).Take(5);

        //     var model = new BountyChartsViewModel()
        //     {
        //         TopIndividuals = topUsers.ToList(),
        //         TopOrgs = topOrgs.ToList()
        //     };

        //     return View(model);
        // }
    }
}
