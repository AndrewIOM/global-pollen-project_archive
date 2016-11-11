using System.ComponentModel.DataAnnotations;

namespace GlobalPollenProject.App.Models
{
    public class NewAppUser
    {
        [Required]
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Forename(s)")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Organisation")]
        public string Organisation { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        [Display(Name = "Confirm Email")]
        public string EmailConfirmation { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}