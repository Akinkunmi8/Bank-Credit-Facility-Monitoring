using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
{
    public class ConfirmedEventHandler : IEventHandler<ConfirmedEvent>
    {
        private readonly INotificationService _notificationService;

        public ConfirmedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(ConfirmedEvent evt)
        {
            await _notificationService.SendApprovalEmailAsync(evt.AppId);
        }
    }
}
