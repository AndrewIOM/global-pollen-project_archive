using Microsoft.AspNetCore.Mvc;

namespace GlobalPollenProject.WebUI.Controllers
{
    [ApiVersionNeutral]
    public class OrganisationController : Controller
    {

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
