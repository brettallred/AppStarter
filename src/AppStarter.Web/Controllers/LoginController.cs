
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using AppStarter.Models;
using AppStarter.ViewModels.Account;

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

            WebContext.SignIn(user.Id, "Admin", model.RememberMe);
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

        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}
