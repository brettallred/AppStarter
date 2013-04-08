using AppStarter.Enums;

namespace AppStarter.Models
{
    public class UserAccount
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserRoles Role { get; set; }

        public string Password
        {
            get { return PasswordHash; }
            set { PasswordHash = EncryptPassword(value); }
        }

        public string PasswordHash { get; private set; }

        public bool PasswordIsValid(string password)
        {
            bool matches = BCrypt.Net.BCrypt.Verify(password, Password);
            return matches;
        }

        private string EncryptPassword(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

    }
}