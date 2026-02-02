using Credit_Monitor.APIDto;

namespace Credit_Monitor.Interfaces
{
    public interface ICovenantTrackerAPIService
    {
        Task<CovenantResponseDto?> GetCovenantDetails(Guid appId);
    }
}
