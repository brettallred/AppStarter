using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppStarter.ViewModels.Account
{
    public class ForgotPasswordModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
    }
}