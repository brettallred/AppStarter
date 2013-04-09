using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppStarter.ViewModels.Account
{
    public class UserAccountModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; }
    }
}