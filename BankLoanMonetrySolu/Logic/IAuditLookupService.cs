namespace BankLoanMonetrySolu.Logic
{
    public interface IAuditLookupService
    {
        Task<string> GetUserEmailByAppIdAsync(Guid appId);
        Task<string> GetUserEmailByUserIdAsync(string userId);
    }

}
