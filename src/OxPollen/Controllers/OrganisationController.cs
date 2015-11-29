using Microsoft.AspNet.Mvc;
using OxPollen.Models;
using OxPollen.ViewModels.Organisation;
using System.Linq;

namespace OxPollen.Controllers
{
    public class OrganisationController : Controller
    {
        public OxPollenDbContext _context;
        public OrganisationController(OxPollenDbContext context)
        {
            _context = context;
        }

        public IActionResult GetAll()
        {
            var result = _context.Organisations.Select(m => new OrganisationViewModel()
            {
                Id = m.OrganisationId,
                Name = m.Name
            }).ToList();
            return Ok(result);
        }

        public IActionResult Search(string searchTerm)
        {
            var result = _context.Organisations.Where(m => m.Name.Contains(searchTerm))
                .Select(m => new OrganisationViewModel()
            {
                Id = m.OrganisationId,
                Name = m.Name
            }).ToList();
            return Ok(result);
        }
    }
}
