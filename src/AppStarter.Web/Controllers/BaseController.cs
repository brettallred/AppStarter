using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using AppStarter.Infrastructure;
using AppStarter.Infrastructure.RavenDB;
using Raven.Client;

namespace AppStarter.Controllers
{
    public class BaseController : Controller
    {
        public IDocumentSession Raven { get; set; }

        public IHttpContextWrapper WebContext { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Raven = (IDocumentSession)HttpContext.Items["CurrentRequestRavenSession"];
            WebContext = new DefaultHttpContextWrapper();


            // This is used to get roles into
            if (User != null && User.Identity.IsAuthenticated)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    var roles = ticket.UserData.Split(',').Select(x => x.Trim()).ToArray();
                    var identity = new GenericIdentity(ticket.Name);
                    Session["Roles"] = roles; // This is for fluent validation.  Need to do a better way
                    HttpContext.User = new GenericPrincipal(identity, roles);
                }
            }
        }

        #region Commands

        protected void Execute(Command cmd)
        {
            cmd.Raven = Raven;
            cmd.Execute();
        }

        protected TResult Execute<TResult>(Command<TResult> cmd)
        {
            Execute((Command)cmd);
            return cmd.Result;
        }

        #endregion

        #region Queries

        protected TResult Query<TResult>(Query<TResult> cmd)
        {
            cmd.Raven = Raven;
            cmd.Execute();
            return cmd.Result;
        }

        #endregion
    }
}
