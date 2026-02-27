using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanAPIService
    {
        Task<List<FinacleRecord>?> GetPendingLoansByRegion(string region);
        Task<FinacleLoanAndCustomerDto?> GetPendingLoanById(string id);
        Task<FinacleDataResponseDto?> GetPendingLoanByAccountNumber(string AccNum);
        Task<StatusUpdateResponseDto> UpdateLoanStatusAsync(StatusUpdateRequestDto request);
        Task<List<FinacleRecord>?> GetConfirmedLoansByRegion(string region);
        Task<List<FinacleRecord>?> GetApprovedLoansByRegion(string region);
        Task<List<FinacleRecord>?> GetRejectedLoansByRegion(string region);
        Task<List<FinacleRecord>?> GetResolvedLoansByRegion(string region);
        Task<List<ExistingResolutionDto>?> GetExistingRemarksByAppId(string appId);
        Task<SaveRemarkResponseDto> SaveDiscrepancyRemarksAsync(SaveRemarkRequestDto request);
    }
}
