using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using BankLoanMonetrySolu.Services;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
{
    public class RejectedEventHandler : IEventHandler<RejectedEvent>
    {
        private readonly INotificationService _notificationService;

        public RejectedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(RejectedEvent evt)
        {
            await _notificationService.SendApprovalEmailAsync(evt.AppId);
        }
    }

}
