using System.ComponentModel.DataAnnotations;
using AppStarter.Infrastructure.DataAnnotationsAttributes;

namespace AppStarter.ViewModels.Account
{
    public class LogOnModel
    {
        [Required(AllowEmptyStrings = false)]
        [Email]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}