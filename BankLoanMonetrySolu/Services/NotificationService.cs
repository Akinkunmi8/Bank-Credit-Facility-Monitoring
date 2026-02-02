using BankLoanMonetrySolu.Logic;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailServices _emailServices;
        private readonly IAuditLookupService _auditLookup;
        private readonly ILogger<NotificationService> _logger; // 1. Define Logger

        public NotificationService(
            IEmailServices emailServices,
            IAuditLookupService auditLookup,
            ILogger<NotificationService> logger) // 2. Inject Logger
        {
            _emailServices = emailServices;
            _auditLookup = auditLookup;
            _logger = logger;
        }

        public async Task SendApprovalEmailAsync(Guid appId)
        {
            try
            {
                _logger.LogInformation("Processing Approval Email for AppId: {AppId}", appId);
                var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Aborting Approval Email: No email found for AppId {AppId}", appId);
                    return;
                }

                await _emailServices.SendEmailAsync(
                    email,
                    "Application Approved",
                    $"Your application with ID {appId} has been approved."
                );
                _logger.LogInformation("Approval Email successfully sent to {Email} for AppId {AppId}", email, appId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Approval Email for AppId: {AppId}", appId);
            }
        }

        public async Task SendRejectionEmailAsync(Guid appId)
        {
            try
            {
                _logger.LogInformation("Processing Rejection Email for AppId: {AppId}", appId);
                var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Aborting Rejection Email: No email found for AppId {AppId}", appId);
                    return;
                }

                await _emailServices.SendEmailAsync(
                    email,
                    "Application Rejected",
                    $"Your application with ID {appId} was rejected."
                );
                _logger.LogInformation("Rejection Email successfully sent to {Email} for AppId {AppId}", email, appId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Rejection Email for AppId: {AppId}", appId);
            }
        }

        public async Task SendConfirmationEmailAsync(Guid appId)
        {
            try
            {
                _logger.LogInformation("Processing Confirmation Email for AppId: {AppId}", appId);
                var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Aborting Confirmation Email: No email found for AppId {AppId}", appId);
                    return;
                }

                await _emailServices.SendEmailAsync(
                    email,
                    "Application Confirmed",
                    $"Your application with ID {appId} has been confirmed."
                );
                _logger.LogInformation("Confirmation Email successfully sent to {Email} for AppId {AppId}", email, appId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Confirmation Email for AppId: {AppId}", appId);
            }
        }

        public async Task SendOnboardEmailAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Processing Onboarding Email for UserId: {UserId}", userId);
                var email = await _auditLookup.GetUserEmailByUserIdAsync(userId);

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Aborting Onboarding Email: No email found for UserId {UserId}", userId);
                    return;
                }

                await _emailServices.SendEmailAsync(
                    email,
                    "User Onboarding",
                    $"Your onboarding was successful. Your User ID is {userId}."
                );
                _logger.LogInformation("Onboarding Email successfully sent to {Email} for UserId {UserId}", email, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Onboarding Email for UserId: {UserId}", userId);
            }
        }

        public async Task SendTATEmailAsync(Guid appId)
        {
            try
            {
                _logger.LogInformation("Checking TAT Escalation for AppId: {AppId}", appId);
                var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Skipping TAT email for App {AppId} because no recipient email was found.", appId);
                    return;
                }

                await _emailServices.SendEmailAsync(
                    email,
                    "TAT Escalation",
                    $"The application with ID {appId} has exceeded TAT. Kindly take action."
                );
                _logger.LogInformation("TAT Escalation email sent to {Email} for AppId {AppId}", email, appId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during TAT escalation email for AppId: {AppId}", appId);
            }
        }
    }
}