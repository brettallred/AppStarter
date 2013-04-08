using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppStarter
{
    public class Settings
    {
        public string SendGridUserName
        {
            get { return ConfigurationManager.AppSettings.Get("SendGrid.UserName"); }
        }

        public string SendGridPassword
        {
            get { return ConfigurationManager.AppSettings.Get("SendGrid.Password"); }
        }
    }
}