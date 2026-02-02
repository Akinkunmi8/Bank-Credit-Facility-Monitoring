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
    public class OnboardEventHandler : IEventHandler<OnbordEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<OnboardEventHandler> _logger;

        public OnboardEventHandler(
            INotificationService notificationService,
            ILogger<OnboardEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task HandleAsync(OnbordEvent evt)
        {
            _logger.LogInformation("Event Received: {EventName} for UserId: {UserId}",
                nameof(OnbordEvent), evt.UserId);

            try
            {
                // Convert Guid to String here
                await _notificationService.SendOnboardEmailAsync(evt.UserId.ToString());

                _logger.LogInformation("Successfully processed onboarding for UserId: {UserId}", evt.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure handling {EventName} for UserId: {UserId}", nameof(OnbordEvent), evt.UserId);
                throw;
            }
        }
    }
}