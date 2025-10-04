using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Web.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
