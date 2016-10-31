using System.Collections.Generic;
using GlobalPollenProject.App.Models;

namespace GlobalPollenProject.App.Interfaces
{
    public interface ITaxonomyService : IAppService
    {
        void AssessValidity (string family, string genus, string species);
        List<BackboneTaxon> Search (string searchTerm, Rank? rank, string parent = null);
    }
}