﻿using ActionMailer.Net.Mvc;
using AppStarter.Services.SendGridMailService;
using AppStarter.ViewModels.Mail;

namespace AppStarter.Controllers
{
    public class MailController : MailerBase
    {
        public MailController()
        {
            MailSender = new GridMailSender(new Settings());
            From = "AppStarter Support <support@appstarter.com>";
        }


        public EmailResult PasswordChanged(ChangePasswordMessage model)
        {
            To.Add(model.Recipient.Email);
            Subject = "AppStarter password changed";
            return Email("PasswordChanged", model);
        }
    }
}
