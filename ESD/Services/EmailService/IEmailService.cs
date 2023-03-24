using ESD.Models;

namespace ESD.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDio request);
    }
}
