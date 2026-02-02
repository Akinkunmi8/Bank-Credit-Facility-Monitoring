using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface IConventTracker
    {
        Task<LoanCovenantTrackerResponse> GetLoanCovenantTrackerResponsesAsync(string AppId);
        Task<string>EditNonFinaciaConeventTracker(string AppId, EditNonFinancialCovenantDto updateDto);
    }
}
