using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface IConventTracker
    {
        Task<LoanCovenantTrackerResponse> GetLoanCovenantTrackerResponsesAsync(string AppId);
        Task<string>EditNonFinaciaConeventTracker(string AppId, EditNonFinancialCovenantDto updateDto);
        Task<string> AddNonFinaciaConeventTracker(string AppId, AddNonFincialConvDto updateDto);
        Task<FinacialCovenantTrackerResponse> AddFinacialConvenatTracker( string AppId, ConventFinacialDto updateDto);
        Task<BulkFinacialCovenantTrackerResponse> UploadFinancialCovenantExcel(IFormFile file);

    }
}
