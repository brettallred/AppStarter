using ActionMailer.Net.Mvc;
using AppStarter.Services.SendGridMailService;

namespace AppStarter.Controllers
{
    public class MailController : MailerBase
    {
        public MailController()
        {
            MailSender = new GridMailSender(new Settings());
        }
    }
}
