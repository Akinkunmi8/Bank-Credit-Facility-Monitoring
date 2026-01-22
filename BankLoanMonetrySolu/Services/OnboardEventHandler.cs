using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
{
    public class OnboardEventHandler : IEventHandler<OnbordEvent>
    {
        private readonly INotificationService _notificationService;

        public OnboardEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(OnbordEvent evt)
        {
            await _notificationService.SendApprovalEmailAsync(evt.UserId);
        }
    }
}
