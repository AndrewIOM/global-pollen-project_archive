﻿using System.Linq;
using GlobalPollenProject.App.Interfaces;
using GlobalPollenProject.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    public class LeaderboardController : Controller
    {
        private IUserService _service;

        public LeaderboardController(IUserService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var topUsers = _service.ListUsersByScore(10).Result;
            var topClubs = _service.ListClubsByScore(10).Result;
            var model = new LeaderboardViewModel()
            {
                TopIndividuals = topUsers.Select(m => new ScoreViewModel() 
                    { Name = m.FirstName.Substring(0,1) + ". " + m.LastName, Score = m.ScoreTotal  }).ToList(),
                TopOrgs = topClubs.Select(m => new ScoreViewModel() { Name = m.Name, Score = m.TotalScore }).ToList()
            };
            return View(model);
        }
        
    }
}
