using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using ActionMailer.Net;
using SendGrid;
using SendGrid.Transport;

namespace AppStarter.Services.SendGridMailService
{
    public class GridMailSender : IMailSender
    {
        private readonly Settings _settings;

        public GridMailSender(Settings settings)
        {
            _settings = settings;
        }

        public void Send(MailMessage mail)
        {
            //create a new message object
            var email = Mail.GetInstance();

            //set the message recipients
            foreach (var recipient in mail.To)
            {
                if (!String.IsNullOrWhiteSpace(recipient.Address))
                    email.AddTo(recipient.Address);
            }

            if (email.To.Length == 0) // this because of some contact has empty email
                return;

            //set the sender
            email.From = mail.From;

            //set the message body
            email.Html = GetText(mail.AlternateViews[0].ContentStream);

            //set the message subject
            email.Subject = mail.Subject;

            string appRootDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName;
            string uploadDir = Path.Combine(appRootDir, "Uploads");

            var filesToDelete = new List<string>();

            //TODO: FIX PERMISSOPS ISSUE
            try
            {
                foreach (var attachment in mail.Attachments)
                {
                    var bufferLength = (int)attachment.ContentStream.Length;
                    var bytes = new byte[bufferLength];
                    attachment.ContentStream.Read(bytes, 0, bufferLength);

                    string fileName = Path.Combine(uploadDir, attachment.Name);
                    filesToDelete.Add(fileName);
                    using (FileStream file = File.Create(fileName))
                    {
                        file.Write(bytes, 0, bufferLength);
                    }
                    email.AddAttachment(fileName);
                }
            }
            catch (Exception)
            {
            }

            //create an instance of the Web transport mechanism
            SMTP emailSender =
                SMTP.GetInstance(new NetworkCredential(_settings.SendGridUserName, _settings.SendGridPassword), port: 587);

            //send the mail
            if (email.To.Any())
            {
                emailSender.Deliver(email);

            }
            //TODO: delete files after mail sending, transportInstance does not free files
            //foreach (var fileName in filesToDelete)
            //{
            //    File.Delete(fileName);
            //}
        }

        public void SendAsync(MailMessage mail, Action<MailMessage> callback)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        private string GetText(Stream ms)
        {
            ms.Position = 0;
            using (var sr = new StreamReader(ms))
            {
                return sr.ReadToEnd();
            }
        }
    }
}