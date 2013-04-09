using System.ComponentModel.DataAnnotations;
using AppStarter.Infrastructure.DataAnnotationsAttributes;

namespace AppStarter.ViewModels.Account
{
    public class UserAccountModel
    {
        [Required(AllowEmptyStrings = false)]
        [Email]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }
}