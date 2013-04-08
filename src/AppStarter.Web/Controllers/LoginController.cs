
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using AppStarter.Models;
using AppStarter.ViewModels.Account;
using AppStarter.ViewModels.Mail;
using MvcFlash.Core;

namespace AppStarter.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index(string returnUrl)
        {
            return View(new LogOnModel { ReturnUrl = returnUrl });
        }

        public ActionResult Login(LogOnModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("LoginError", "Authentication Failed");
                return View("Index", model);
            }

            UserAccount user = Raven.Query<UserAccount>().FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !user.PasswordIsValid(model.Password))
            {
                ModelState.AddModelError("LoginError", "Authentication Failed");
                return View("Index", model);
            }

            WebContext.SignIn(user.Id, user.Role.ToString(), model.RememberMe);
            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(model.ReturnUrl);
        }

        public ActionResult Logout()
        {
            WebContext.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            var user = Raven.Query<UserAccount>().SingleOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Error", "Email not found.");
            }
            if (!ModelState.IsValid)
                return View(model);

            var newPassword = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);
            user.Password = newPassword;
            Raven.Store(user);
            Raven.SaveChanges();

            new MailController().PasswordChanged(new ChangePasswordMessage {NewPassword = newPassword, Recipient = user}).Deliver();

            Flash.Success("A new password has been emailed to you.");
            return RedirectToAction("Index");
        }
    }
}
