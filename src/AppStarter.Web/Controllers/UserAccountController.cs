using System.Web.Mvc;
using AppStarter.Models;
using AppStarter.ViewModels.Account;

namespace AppStarter.Controllers
{
    public class UserAccountController : BaseController
    {
        //
        // GET: /UserAccount/

        [HttpGet]
        public ActionResult Edit()
        {
            var user = Raven.Load<UserAccount>(WebContext.UserAccountId);
            var model = new UserAccountModel
            {
                FullName = user.FullName,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserAccountModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);
            var user = Raven.Load<UserAccount>(WebContext.UserAccountId);
            user.Email = model.Email;
            user.FullName = model.FullName;
            if(!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }
            Raven.Store(user);
            Raven.SaveChanges();
            model.Password = string.Empty;
            MvcFlash.Core.Flash.Success("Information has been saved.");
            return View("Index", model);
        }

    }
}
