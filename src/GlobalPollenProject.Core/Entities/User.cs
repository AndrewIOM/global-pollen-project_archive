
using GlobalPollenProject.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GlobalPollenProject.Core
{
    public class User : IdentityUser, IEntity
    {
        private User() {}
        public User(string title, string firstName, string lastName)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            BountyScore = 0;
        }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Organisation Organisation { get; private set; }

        public double BountyScore { get; set; }
        public bool RequestedDigitisationRights { get; private set; }

        public bool IsDeleted { get; set; }

        // Business functions
    }
}