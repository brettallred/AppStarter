using System.Linq;
using System.Web.Mvc;
using AppStarter.Models;
using AppStarter.ViewModels.Registration;
using MvcFlash.Core;

namespace AppStarter.Controllers
{
    public class RegistrationsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new RegistrationModel());
        }

        public ActionResult Create(RegistrationModel model)
        {
            if (Raven.Query<UserAccount>().SingleOrDefault(x => x.Email == model.Email) != null)
            {
                ModelState.AddModelError("Error", "User with such email already exists.");
            }
            if (!ModelState.IsValid)
                return View("Index", model);

            var user = new UserAccount
            {
                FullName = model.FullName,
                Password = model.Password,
                Email = model.Email
            };

            Raven.Store(user);
            Raven.SaveChanges();
            Flash.Success("Your account has been created.  You can now login.");

            new MailController().SendUserSignupMessage(model).Deliver();

            return RedirectToAction("Index", "Login");
        }

    }
}
