using Credit_Monitor.APIDto;

namespace Credit_Monitor.Interfaces
{
    public interface ICovenantTrackerAPIService
    {
        Task<CovenantResponseDto?> GetCovenantDetails(Guid appId);
        Task<UpdateCovenantResponseDto?> UpdateNonFinancialCovenant(Guid appId, UpdateCovenantRequestDto request);
        Task<UpdateCovenantResponseDto?> AddNonFinancialCovenant(Guid appId, UpdateCovenantRequestDto request);
    }
}
