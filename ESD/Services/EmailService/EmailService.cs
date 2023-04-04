using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using ESD.Models;
using MailKit.Net.Smtp;

namespace ESD.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public void SendEmail(EmailDio request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("caterina.labadie98@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("EmailUserName").Value, config.GetSection("EmailPassword").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
