using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanService
    {
        Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByIdAsync(string id);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region);
        Task<CATViewModel> GetCustomerLoanDetailsfromCATAsync(string AccNum);
        Task<DiscrepanciesViewModel> ValidateLoanDiscrepancies(string AccNum);
        Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByAccNumAsync(string AccNum);
    }
}
