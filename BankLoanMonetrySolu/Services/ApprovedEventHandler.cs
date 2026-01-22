using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
    
{
    public class ApprovedEventHandler : IEventHandler<ApprovedEvents>
    {
        private readonly INotificationService _notificationService;

        public ApprovedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(ApprovedEvents evt)
        {
            await _notificationService.SendApprovalEmailAsync(evt.AppId);
        }
    }



}
