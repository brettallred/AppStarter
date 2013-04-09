using System.Linq;
using AppStarter.Models;
using Raven.Client;

namespace AppStarter.Helpers.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static UserAccount GetUserByEmail(this IDocumentSession session, string email)
        {
            return session.Query<UserAccount>().FirstOrDefault(u => u.Email == email);
        }
    }
}