using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using Organic_Food_MVC_Project.Services.Interfaces;
using Microsoft.Extensions.Options;
using Organic_Food_MVC_Project.Helpers;
using MailKit.Net.Smtp;

namespace Organic_Food_MVC_Project.Services
{
    public class EmailService:IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.From, _appSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

}
