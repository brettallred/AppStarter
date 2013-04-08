using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppStarter.ViewModels.Registration
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}