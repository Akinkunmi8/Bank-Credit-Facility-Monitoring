using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;

namespace BankLoanMonetrySolu.Services
{
    public class TATEventHandler : IEventHandler<TATEvent>
    {
        private readonly INotificationService _notificationService;

        public TATEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task HandleAsync(TATEvent evt)
        {
            await _notificationService.SendTATEmailAsync(evt.AppId);
        }
    }



}
