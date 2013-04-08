using System.Web;

namespace AppStarter.Infrastructure
{
    public interface IHttpContextWrapper
    {
        void AddCookie(HttpCookie cookie);
        void SignOut();
        void SignIn(string accountId, string roles, bool isRememberMe);
        bool IsAuthenticated { get; }
        string UserAccountId { get; }
    }
}
