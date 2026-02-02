using Contracts.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class EmailService : IEmailServices
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string receiver, string subject, string body)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = _config["EmailSettings:Port"];
            var senderEmail = _config["EmailSettings:SenderEmail"];

            _logger.LogInformation("Attempting to send email to {Receiver}. Subject: {Subject} via {Server}:{Port}",
                receiver, subject, smtpServer, port);

            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(receiver);

                using var smtp = new SmtpClient(smtpServer, int.Parse(port))
                {
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:Username"],
                        _config["EmailSettings:Password"]
                    ),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail);

                _logger.LogInformation("Email successfully dispatched to {Receiver}", receiver);
            }
            catch (SmtpException ex)
            {
                // Structured logging for SMTP-specific codes (e.g., 550, 535)
                _logger.LogError(ex, "SMTP Protocol Error occurred. Status Code: {StatusCode}. Receiver: {Receiver}",
                    ex.StatusCode, receiver);

                throw new Exception($"SMTP Error: {ex.StatusCode} - {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General failure in EmailService while sending to {Receiver}", receiver);
                throw new Exception($"Email sending failed: {ex.Message}", ex);
            }
        }
    }
}