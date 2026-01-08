using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanRepository
    {
        Task<PendingConfirmedLoan?> GetPendingConfirmedLoanByIdAsync(string id);
        Task<PendingConfirmedLoan?> GetPendingConfirmedLoanByAccNumAsync(string AccNum);
        Task<IEnumerable<PendingConfirmedLoan>> GetPendingConfirmedLoansByRegionAsync(string region);
        Task<CAT?> GetCustomerLoanDetailsfromCAT(string AccNum);
    }
}
