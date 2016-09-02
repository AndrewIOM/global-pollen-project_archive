using Microsoft.AspNetCore.Mvc;
using Im.Acm.Pollen.Data.Concrete;
using Im.Acm.Pollen.ViewModels.Organisation;
using System.Linq;

namespace Im.Acm.Pollen.Controllers
{
    public class OrganisationController : Controller
    {
        public PollenDbContext _context;
        public OrganisationController(PollenDbContext context)
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
