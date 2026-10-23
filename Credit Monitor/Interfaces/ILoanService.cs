using Credit_Monitor.APIDto;
using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanService
    {
        Task<PendingLoanWithCustomerDetailsViewModel?> GetPendingConfirmedLoanByIdAsync(string id);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region);
        Task<CATViewModel> GetCustomerLoanDetailsfromCATAsync(string AccNum);
        Task<DiscrepanciesViewModel> ValidateLoanDiscrepancies(string AccNum);
        Task<FinacleDataViewModel?> GetPendingConfirmedLoanByAccNumAsync(string AccNum);
        Task<(bool isSuccess, string message)> ProcessStatusUpdate(Guid appId, string actionType, DateTime? deadline);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetConfirmedLoanByRegion(string region);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetApprovedLoanByRegion(string region);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetRejectedLoanByRegion(string region);
        Task<List<ResolutionItem>> GetSavedResolutionsMapped(Guid? appId);
        //Task<List<ResolutionItem>> GetSavedResolutionsMapped(Guid appId);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetResolvedLoanByRegion(string region);
        Task<IEnumerable<PendingConfirmedLoanViewModel>> GetCMOReviewLoanByRegion(string region);

        Task<(int SuccessCount, int FailureCount)> ProcessBulkResolutionsAsync(Guid appId, List<ResolutionItem> resolutions);
        Task<(int SuccessCount, int FailureCount)> ProcessBulkResolutionResponsesAsync(Guid appId,
        List<ResolutionItem> resolutions);
        Task<ServiceResponseDto> SubmitResolutionForReviewAsync(Guid appId);
        Task<FullDashboardReportDataDto?> GetFullReportPreview(string region,string role);
    }
}
