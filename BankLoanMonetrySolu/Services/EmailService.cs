using BankLoanMonetrySolu.IServices;
using System.Net;
using System.Net.Mail;

namespace BankLoanMonetrySolu.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            using var smtp = new SmtpClient(
                _config["Email:Host"],
                int.Parse(_config["Email:Port"])
            )
            {
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                ),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }

}
