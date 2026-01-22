using BankLoanMonetrySolu.Logic;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailServices _emailServices;
        private readonly IAuditLookupService _auditLookup;

        public NotificationService(
            IEmailServices emailServices,
            IAuditLookupService auditLookup)
        {
            _emailServices = emailServices;
            _auditLookup = auditLookup;
        }

        public async Task SendApprovalEmailAsync(Guid appId)
        {
            var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

            await _emailServices.SendEmailAsync(
                email,
                "Application Approved",
                $"Your application with ID {appId} has been approved."
            );
        }

        public async Task SendRejectionEmailAsync(Guid appId)
        {
            var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

            await _emailServices.SendEmailAsync(
                email,
                "Application Rejected",
                $"Your application with ID {appId} was rejected."
            );
        }

        public async Task SendConfirmationEmailAsync(Guid appId)
        {
            var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

            await _emailServices.SendEmailAsync(
                email,
                "Application Confirmed",
                $"Your application with ID {appId} has been confirmed."
            );
        }

        public async Task SendOnboardEmailAsync(string userId)
        {
            var email = await _auditLookup.GetUserEmailByUserIdAsync(userId);

            await _emailServices.SendEmailAsync(
                email,
                "User Onboarding",
                $"Your onboarding was successful. Your User ID is {userId}."
            );
        }

        public async Task SendTATEmailAsync(Guid appId)
        {
            var email = await _auditLookup.GetUserEmailByAppIdAsync(appId);

            await _emailServices.SendEmailAsync(
                email,
                "TAT Escalation",
                $"The application with ID {appId} has exceeded TAT. Kindly take action."
            );
        }
    }

}


