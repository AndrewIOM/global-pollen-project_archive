using Microsoft.AspNet.Identity.EntityFramework;

namespace OxPollen.Models
{
    public class AppUser : IdentityUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Organisation Organisation { get; set; }
        public double BountyScore { get; set; }

        //Methods
        public string FullName()
        {
            return Title + " " + FirstName + " " + LastName;
        }
    }
}
