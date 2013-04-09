using System.ComponentModel.DataAnnotations;
using AppStarter.Infrastructure.DataAnnotationsAttributes;

namespace AppStarter.ViewModels.Account
{
    public class ForgotPasswordModel
    {
        [Required(AllowEmptyStrings = false)]
        [Email]
        public string Email { get; set; }
    }
}