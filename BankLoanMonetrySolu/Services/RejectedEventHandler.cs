using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using BankLoanMonetrySolu.Services;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class RejectedEventHandler : IEventHandler<RejectedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<RejectedEventHandler> _logger;

        public RejectedEventHandler(
            INotificationService notificationService,
            ILogger<RejectedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(RejectedEvent evt)
        {
            _logger.LogInformation("Event Received: {EventName} for Application ID: {AppId}",
                nameof(RejectedEvent), evt.AppId);

            try
            {
                // FIX: Corrected from SendApprovalEmailAsync to SendRejectionEmailAsync
                await _notificationService.SendRejectionEmailAsync(evt.AppId);

                _logger.LogInformation("Successfully processed {EventName} for AppId: {AppId}. Rejection notice sent.",
                    nameof(RejectedEvent), evt.AppId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure handling {EventName} for AppId: {AppId}. Customer was not notified of rejection.",
                    nameof(RejectedEvent), evt.AppId);

                // Throwing ensures the message is returned to the queue/dlq for troubleshooting
                throw;
            }
        }
    }
}