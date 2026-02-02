using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class ConfirmedEventHandler : IEventHandler<ConfirmedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<ConfirmedEventHandler> _logger;

        public ConfirmedEventHandler(
            INotificationService notificationService,
            ILogger<ConfirmedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(ConfirmedEvent evt)
        {
            _logger.LogInformation("Event Received: {EventName} for Application ID: {AppId}",
                nameof(ConfirmedEvent), evt.AppId);

            try
            {
                // FIX: Changed from SendApprovalEmailAsync to SendConfirmationEmailAsync
                // to match the intent of the 'Confirmed' event.
                await _notificationService.SendConfirmationEmailAsync(evt.AppId);

                _logger.LogInformation("Successfully processed {EventName} for AppId: {AppId}",
                    nameof(ConfirmedEvent), evt.AppId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling {EventName} for AppId: {AppId}",
                    nameof(ConfirmedEvent), evt.AppId);

                // Re-throwing depends on whether your event bus (like RabbitMQ) 
                // supports retry policies/dead-letter queues.
                throw;
            }
        }
    }
}