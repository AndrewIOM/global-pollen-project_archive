using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GlobalPollenProject.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Organisation Organisation { get; set; }

        public double BountyScore { get; set; }
        public bool RequestedDigitisationRights { get; set; }

        //Methods
        public string FullName()
        {
            return Title + " " + FirstName + " " + LastName;
        }
    }
}
