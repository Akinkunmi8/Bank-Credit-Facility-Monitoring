using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class ApprovedEventHandler : IEventHandler<ApprovedEvents>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<ApprovedEventHandler> _logger;

        public ApprovedEventHandler(
            INotificationService notificationService,
            ILogger<ApprovedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(ApprovedEvents evt)
        {
            _logger.LogInformation("Event Received: {EventName} for Application ID: {AppId}",
                nameof(ApprovedEvents), evt.AppId);

            try
            {
                // Passing the logic to the notification service
                await _notificationService.SendApprovalEmailAsync(evt.AppId);

                _logger.LogInformation("Successfully handled {EventName} for AppId: {AppId}",
                    nameof(ApprovedEvents), evt.AppId);
            }
            catch (Exception ex)
            {
                // Background tasks must catch their own exceptions to prevent the worker from crashing
                _logger.LogError(ex, "Critical failure handling {EventName} for AppId: {AppId}",
                    nameof(ApprovedEvents), evt.AppId);
            }
        }
    }
}