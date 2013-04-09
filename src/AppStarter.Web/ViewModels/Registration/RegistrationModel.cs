using System.ComponentModel.DataAnnotations;
using AppStarter.Infrastructure.DataAnnotationsAttributes;

namespace AppStarter.ViewModels.Registration
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [Email]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}