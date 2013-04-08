using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace AppStarter.Infrastructure
{
    public class DefaultHttpContextWrapper : IHttpContextWrapper
    {
        public IPrincipal Principal
        {
            get { return HttpContext.Current.User; }
        }

        public void AddCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();

            HttpContext.Current.Session.Abandon();

            var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);

            var cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
        }

        public void SignIn(string accountId, string roles, bool isRememberMe)
        {
            var authTicket = new FormsAuthenticationTicket(
                1,
                accountId,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                isRememberMe,
                roles
                );
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (isRememberMe)
                authCookie.Expires = DateTime.Now.AddYears(1);

            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public bool IsAuthenticated { get { return HttpContext.Current.Request.IsAuthenticated; } }
        public string UserAccountId { get { return HttpContext.Current.User.Identity.Name; } }
    }
}