using GlobalPollenProject.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GlobalPollenProject.Core.State
{
    public class UserState : IdentityUser, IEntity
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Organisation Organisation { get; private set; }
        public double BountyScore { get; set; }
        public bool RequestedDigitisationRights { get; private set; }
        public bool IsDeleted { get; set; }
    }
}