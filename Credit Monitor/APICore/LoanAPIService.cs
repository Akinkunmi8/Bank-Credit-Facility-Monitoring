using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json;

namespace Credit_Monitor.APICore
{
    public class LoanAPIService : ILoanAPIService
    {
        private readonly IBackendAPIService _apiService;
        private readonly ILogger<LoanAPIService> _logger; // 1. Add Logger

        public LoanAPIService(IBackendAPIService apiService, ILogger<LoanAPIService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<FinacleRecord>?> GetPendingLoansByRegion(string region)
        {
            var urlPath = $"/api/Finacle/by-region/{region}";
            var result = await _apiService.GetAsync<FinacleResponseWrapper>(urlPath);

            if (result == null || !result.isSuccess)
            {
                _logger.LogWarning("Loan API: Failed to fetch Pending loans for {Region}. Message: {Msg}",
                    region, result?.responseMessage ?? "Null Response");
                return null;
            }

            _logger.LogInformation("Loan API: Retrieved {Count} Pending loans for {Region}",
                result.dataList?.Count ?? 0, region);
            return result.dataList ?? [];
        }

        public async Task<List<FinacleRecord>?> GetConfirmedLoansByRegion(string region)
        {
            var urlPath = $"api/Finacle/confirmed?region={region}";
            var result = await _apiService.GetAsync<FinacleResponseWrapper>(urlPath);

            if (result == null || !result.isSuccess)
            {
                _logger.LogWarning("Loan API: Failed to fetch Confirmed loans for {Region}", region);
                return null;
            }

            _logger.LogInformation("Loan API: Retrieved {Count} Confirmed loans for {Region}",
                result.dataList?.Count ?? 0, region);
            return result.dataList ?? [];
        }

        public async Task<FinacleLoanAndCustomerDto?> GetPendingLoanById(string id)
        {
            var urlPath = $"api/Dashboard/View-LoanAndCustomerDetails?appId={id}";
            var result = await _apiService.GetAsync<FinacleSingleResponseWrapper>(urlPath);

            if (result == null || !result.isSuccess)
            {
                _logger.LogWarning("Loan API: Detailed fetch failed for AppId: {AppId}", id);
                return null;
            }

            // 2. Log the full JSON of the loan details to verify mapping
            var jsonDetails = JsonConvert.SerializeObject(result.data, Formatting.Indented);
            _logger.LogInformation("Loan API: Success fetching AppId {AppId}. Data: {Json}", id, jsonDetails);

            return result.data;
        }

        public async Task<FinacleDataResponseDto?> GetPendingLoanByAccountNumber(string AccNum)
        {
            var urlPath = $"api/Finacle/CompareData?acctNum={AccNum}";
            var result = await _apiService.GetAsync<FinacleDataResponseDto>(urlPath);

            if (result == null || !result.IsSuccess)
            {
                _logger.LogWarning("Loan API: CompareData failed for Account: {AccNum}", AccNum);
                return null;
            }

            _logger.LogInformation("Loan API: CompareData success for {AccNum}. Result: {Json}",
                AccNum, JsonConvert.SerializeObject(result));
            return result;
        }

        public async Task<StatusUpdateResponseDto> UpdateLoanStatusAsync(StatusUpdateRequestDto request)
        {
            var urlPath = "/api/Dashboard/update-status";

            // 3. Log the outgoing request body (essential for debugging update failures)
            _logger.LogInformation("Loan API: Updating status for AppId {Id} to {Status}",
                request.ApplicationId, request.Action);

            var response = await _apiService.PostAsync<StatusUpdateResponseDto>(request, urlPath);

            _logger.LogInformation("Loan API: Update Status result: {Success} | {Message}",
                response.Success, response.Message);

            return response;
        }

        public async Task<List<FinacleRecord>?> GetApprovedLoansByRegion(string region)
        {
            var urlPath = $"api/Finacle/approved?region={region}";

            _logger.LogInformation("Loan API: Fetching Approved loans for region: {Region}", region);

            try
            {
                var result = await _apiService.GetAsync<FinacleResponseWrapper>(urlPath);

                if (result == null || !result.isSuccess)
                {
                    _logger.LogWarning("Loan API: Failed to fetch Approved loans for {Region}. Status: {Status}",
                        region, result?.responseMessage ?? "No Response");
                    return null;
                }

                _logger.LogInformation("Loan API: Retrieved {Count} Approved loans for {Region}",
                    result.dataList?.Count ?? 0, region);

                return result.dataList ?? new List<FinacleRecord>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loan API: Critical error fetching Approved loans for {Region}", region);
                return null;
            }
        }

        public async Task<List<FinacleRecord>?> GetRejectedLoansByRegion(string region)
        {
            var urlPath = $"api/Finacle/reconfirmed/by-region/{region}";

            _logger.LogInformation("Loan API: Fetching Rejected/Reconfirmed loans for region: {Region}", region);

            try
            {
                var result = await _apiService.GetAsync<FinacleResponseWrapper>(urlPath);

                if (result == null || !result.isSuccess)
                {
                    _logger.LogWarning("Loan API: Failed to fetch Rejected loans for {Region}. Path used: {Url}",
                        region, urlPath);
                    return null;
                }

                _logger.LogInformation("Loan API: Retrieved {Count} Rejected loans for {Region}",
                    result.dataList?.Count ?? 0, region);

                return result.dataList ?? new List<FinacleRecord>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loan API: Critical error fetching Rejected loans for {Region}", region);
                return null;
            }
        }

    }
}
