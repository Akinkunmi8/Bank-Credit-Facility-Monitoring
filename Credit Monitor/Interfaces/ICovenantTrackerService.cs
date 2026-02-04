using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModels;

namespace Credit_Monitor.Interfaces
{
    public interface ICovenantTrackerService
    {
        Task<CovenantViewModel?> GetCovenantDetailsAsync(Guid appId);
        Task<UpdateCovenantResponseDto> UpdateCovenant(NonFinancialCovenantVM model, Guid appId);
        Task<UpdateCovenantResponseDto> CreateCovenant(NonFinancialCovenantVM model, Guid appId);
    }
}
