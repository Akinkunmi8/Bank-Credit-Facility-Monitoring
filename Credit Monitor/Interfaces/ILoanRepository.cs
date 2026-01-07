using Credit_Monitor.Models;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanRepository
    {
        Task<PendingConfirmedLoan?> GetPendingConfirmedLoanByIdAsync(string id);
        Task<IEnumerable<PendingConfirmedLoan>> GetPendingConfirmedLoansByRegionAsync(string region);
    }
}
