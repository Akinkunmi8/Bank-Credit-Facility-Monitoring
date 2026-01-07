using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanService
    {
        Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByIdAsync(string id);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region);
    }
}
