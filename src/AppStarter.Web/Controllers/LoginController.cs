
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using AppStarter.Enums;
using AppStarter.Models;
using AppStarter.ViewModels.Account;
using AppStarter.ViewModels.Mail;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return View("Index", new LogOnModel { ReturnUrl = returnUrl });
            }

            string email = result.ExtraData["email"];

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("LoginError", "Authentication Failed");
                return View("Index", new LogOnModel());
            }

            //var user = _repository.SelectAll().FirstOrDefault(u => u.Email == email);

            //if (user == null)
            //{
            //    user = new UserAccount
            //    {
            //        FullName = String.Empty,
            //        Password = String.Empty,
            //        Email = email,
            //        Roles = new List<UserRoles> { UserRoles.User }
            //    };

            //    _repository.Insert(user);

            //    Flash.Success("Your account has been created.  You can now login.");
            //}
            //else
            //{
            //    bool exists = user.ExternalLogins.Any(x => x.Type == ExternalLoginType.Google && x.UserName == result.UserName);

            //    if (!exists)
            //    {
            //        user.ExternalLogins.Add(new ExternalLogin
            //        {
            //            AccessToken = String.Empty,
            //            Type = ExternalLoginType.Google,
            //            UserName = result.UserName
            //        });

            //        _repository.Update(user);
            //    }
            //}

            //WebContext.SignIn(user, false);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }


        #region Helpers

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
