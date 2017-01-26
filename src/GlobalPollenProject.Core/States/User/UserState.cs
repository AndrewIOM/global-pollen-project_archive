using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GlobalPollenProject.Core
{
    public class UserState : IdentityUser
    {
        public bool RequestedDigitisationRights { get; private set; }
        public ResearcherProfile Profile { get; set; }

        public double CurrentScore { get; set; }
        public List<ScoreRecord> ScoreHistory { get; set; }
        public List<OrganisationId> Organisations { get; set; }
    }
}