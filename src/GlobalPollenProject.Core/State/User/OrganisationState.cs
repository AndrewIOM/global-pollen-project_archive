using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.State
{
    public class OrganisationState : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public List<User> Members { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebUrl { get; set; }
    }
}