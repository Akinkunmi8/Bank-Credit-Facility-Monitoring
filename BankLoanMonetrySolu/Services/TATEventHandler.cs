using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class TATEventHandler : IEventHandler<TATEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<TATEventHandler> _logger;

        public TATEventHandler(
            INotificationService notificationService,
            ILogger<TATEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(TATEvent evt)
        {
            _logger.LogInformation("TAT Escalation Triggered: {EventName} for AppId: {AppId}",
                nameof(TATEvent), evt.AppId);

            try
            {
                // This usually notifies a supervisor or manager
                await _notificationService.SendTATEmailAsync(evt.AppId);

                _logger.LogInformation("Successfully dispatched TAT Escalation notice for AppId: {AppId}", evt.AppId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process TAT Escalation for AppId: {AppId}. Manual intervention may be required.", evt.AppId);

                // Re-throw to ensure the event isn't lost if the message broker supports retries
                throw;
            }
        }
    }
}