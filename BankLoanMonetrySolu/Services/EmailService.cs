using Contracts.Notifications;
using System.Net;
using System.Net.Mail;

namespace BankLoanMonetrySolu.Services
{
    public class EmailService : IEmailServices
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string receiver, string subject, string body)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(receiver);

                using var smtp = new SmtpClient(
                    _config["EmailSettings:SmtpServer"],
                    int.Parse(_config["EmailSettings:Port"])
                )
                {
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:Username"],
                        _config["EmailSettings:Password"]
                    ),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail);
            }
            catch (SmtpException ex)
            {
                // Gmail / SMTP-specific issues
                throw new Exception($"SMTP Error: {ex.StatusCode} - {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // General failures
                throw new Exception($"Email sending failed: {ex.Message}", ex);
            }
        }
    }
}
