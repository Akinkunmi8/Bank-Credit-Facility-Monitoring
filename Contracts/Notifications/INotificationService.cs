namespace Contracts.Notifications
{
    public interface INotificationService
    {
        Task SendOnboardEmailAsync(string userId);
        Task SendApprovalEmailAsync(/*string userId,*/ Guid appId);
        Task SendRejectionEmailAsync(/*string userId,*/ Guid appId);
        Task SendConfirmationEmailAsync(/*string userId*/ Guid appId);
        Task SendTATEmailAsync(/*string userId*/ Guid appId);
    }

    public interface IEmailServices
    {
        Task SendEmailAsync(string receiver, string subject, string body);
    }
}
