using Credit_Monitor.ViewModels;

namespace Credit_Monitor.Interfaces
{
    public interface ICovenantTrackerService
    {
        Task<CovenantViewModel?> GetCovenantDetailsAsync(Guid appId);
    }
}
