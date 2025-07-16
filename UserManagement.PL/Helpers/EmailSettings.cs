using System.Net;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Options;
using UserManagement.PL.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace UserManagement.PL.Helpers
{
    public class EmailSettings : IEmailSettings
    {
        private readonly MailSettings _options;

        public EmailSettings(IOptions<MailSettings> options) {
            _options = options.Value;
        }

        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.SenderEmail),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            var builder = new BodyBuilder();
            builder.HtmlBody = email.Body;
            mail.Body = builder.ToMessageBody();
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.SenderEmail));
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.SenderEmail, _options.SenderPassword);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }

        //public static void SendEmail(Email email)
        //{
        //    var client = new SmtpClient("smtp.gmail.com", 587);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("faketosite@gmail.com", "jvftemomjsxssiny");
        //    client.Send("faketosite@gmail.com", email.To, email.Subject, email.Body);
        //}
    }
}
