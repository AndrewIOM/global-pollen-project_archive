using Microsoft.AspNet.Mvc;
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
            var orgs = _userService.GetOrganisations();

            var topOrgs = orgs.Select(m => new BountyViewModel()
            {
                Bounty = m.Members.Select(n => n.BountyScore).Sum(),
                Name = m.Name
            });

            var topUsers = users.Select(m => new BountyViewModel()
            {
                Bounty = m.BountyScore,
                Name = m.FullName()
            });

            var model = new BountyChartsViewModel()
            {
                TopIndividuals = topUsers.ToList(),
                TopOrgs = topOrgs.ToList()
            };

            return View(model);
        }
    }
}
