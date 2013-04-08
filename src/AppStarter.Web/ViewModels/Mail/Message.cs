using AppStarter.Models;

namespace AppStarter.ViewModels.Mail
{
    public abstract class Message
    {
        public UserAccount Recipient { get; set; }
    }
}