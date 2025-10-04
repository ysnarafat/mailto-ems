using System.ComponentModel.DataAnnotations;

namespace EmailMarketing.Web.Models.Account
{
    public class ChangeDefaultPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //private IEncryptDecryptService _encryptDecryptService;

        //public ChangeDefaultPasswordViewModel()
        //{
        //    _encryptDecryptService = Startup.AutofacContainer.Resolve<IEncryptDecryptService>();
        //}

        //public ChangeDefaultPasswordViewModel(IEncryptDecryptService encryptDecryptService)
        //{
        //    _encryptDecryptService = encryptDecryptService;
        //}

        //public string EncryptPassword()
        //{
        //    var key = "E546C8DF278CD5931069B522E695D4F2";
        //    var result = _encryptDecryptService.EncryptString(this.Password, key);
        //    return result;
        //}

        //public string DecryptPassword(string value)
        //{
        //    var key = "E546C8DF278CD5931069B522E695D4F2";
        //    var result = _encryptDecryptService.DecryptString(value, key);
        //    return result;
        //}
    }
}
