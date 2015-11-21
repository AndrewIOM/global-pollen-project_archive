using Microsoft.AspNet.Mvc;
using Microsoft.Dnx.Runtime;
using OxPollen.Models;
using OxPollen.Services.Abstract;
using OxPollen.ViewModels;
using System.Linq;

namespace OxPollen.Controllers
{
    public class BountyController : Controller
    {
        private readonly IIdentificationService _idService;
        private readonly IUserService _userService;

        public BountyController(IIdentificationService id, IUserService user)
        {
            _idService = id;
            _userService = user;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var users = _userService.GetAll();
            var topOrgs = users.GroupBy(m => m.Organisation)
                .OrderByDescending(m => m.Select(n => n.Bounty).Sum()).Take(5);
            var topUsers = users.OrderByDescending(m => m.Bounty).Take(5);
            var model = new BountyChartsViewModel()
            {
                TopIndividuals = topUsers.Select(m => new BountyViewModel()
                {
                    Name = m.FullName,
                    Bounty = m.Bounty
                }).ToList(),
                TopOrgs = topOrgs.Select(m => new BountyViewModel()
                {
                    Name = m.Key,
                    Bounty = m.Select(o => o.Bounty).Sum()
                }).ToList()
            };
            return View(model);
        }
    }
}
