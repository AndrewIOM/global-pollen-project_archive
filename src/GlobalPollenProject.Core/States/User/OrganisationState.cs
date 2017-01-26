using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class OrganisationState : IState<Organisation>
    {
        public OrganisationId Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebUrl { get; set; }

        public List<UserId> Members { get; set; }

        public Organisation ToEntity(IState<Organisation> state)
        {
            throw new NotImplementedException();
        }
    }
}