using System;
using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Web.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions.")]
        public bool isChecked { get; set; }
    }
}
