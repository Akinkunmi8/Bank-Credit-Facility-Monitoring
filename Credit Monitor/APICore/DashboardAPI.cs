using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; // Import Newtonsoft for JSON serialization

namespace Credit_Monitor.APICore
{
    public class DashboardAPI : IDashboardAPI
    {
        private readonly IBackendAPIService _apiService;
        private readonly ILogger<DashboardAPI> _logger;

        public DashboardAPI(IBackendAPIService apiService, ILogger<DashboardAPI> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<int> GetTotalPending(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/pending?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);

            // Log the simple integer value
            _logger.LogInformation("Dashboard Data - Region: {Region} | Pending Count: {Value}", region, result);

            return result;
        }

        public async Task<int> GetTotalConfirmed(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/confirmed?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);

            _logger.LogInformation("Dashboard Data - Region: {Region} | Confirmed Count: {Value}", region, result);

            return result;
        }

        public async Task<int> GetTotalRejected(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/reconfirmed/{region}";
            var result = await _apiService.GetAsync<int>(urlPath);

            _logger.LogInformation("Dashboard Data - Region: {Region} | Rejected Count: {Value}", region, result);

            return result;
        }

        public async Task<int> GetTotalApproved(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/approved?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);

            _logger.LogInformation("Dashboard Data - Region: {Region} | Approved Count: {Value}", region, result);

            return result;
        }

        public async Task<int> GetTotalResolved(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/resolved?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);

            _logger.LogInformation("Dashboard Data - Region: {Region} | Resolved Count: {Value}", region, result);

            return result;
        }

        public async Task<int> GetWithinTAT(string region)
        {
            var urlPath = $"/api/Dashboard/tat/within/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Serialize the full TAT DTO so you can see all properties (isSuccessful, tatCount, etc.)
            var jsonResult = JsonConvert.SerializeObject(result);
            _logger.LogInformation("TAT Data Received - Region: {Region} | JSON: {Data}", region, jsonResult);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetBeyondTAT(string region)
        {
            var urlPath = $"/api/Dashboard/tat/beyond/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetWithinTATUnReview(string region)
        {
            var urlPath = $"/api/Dashboard/tatcount/within/Unreview/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetWithinTATUnResolve(string region)
        {
            var urlPath = $"/api/Dashboard/tatcount/within/Unresolve/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetBeyondTATUnReview(string region)
        {
            var urlPath = $"/api/Dashboard/tatcount/within/Unresolve/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetBeyondTATUnApprove(string region)
        {
            var urlPath = $"/api/Dashboard/tatcount/beyond/UnApprove/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<int> GetBeyondTATUnResolve(string region)
        {
            var urlPath = $"/api/Dashboard/tatcount/beyond/Unresolve/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);

            if (result == null || !result.isSuccessful)
            {
                _logger.LogWarning("TAT Request Failed - Region: {Region} | Path: {Path}", region, urlPath);
                return 0;
            }

            // Log using Newtonsoft to verify the string value before parsing
            _logger.LogInformation("Beyond TAT Data - Region: {Region} | Value: {Value}", region, result.tatCount);

            return Int32.Parse(result.tatCount);
        }

        public async Task<List<TATOutputResponse>?> GetBeyondTATUnreviewedLoansByRegion(string region)
        {
            // The specific URL from your requirement
            var urlPath = $"/api/Dashboard/data/beyond/unreview/{region}";

            try
            {
                // We call the wrapper because of the $values JSON structure
                var result = await _apiService.GetAsync<TATOutputResponseWrapper>(urlPath);

                if (result == null || result.DataList == null)
                {
                    _logger.LogWarning("Loan API: No data returned for region {Region} at {Url}", region, urlPath);
                    return new List<TATOutputResponse>();
                }

                _logger.LogInformation("Loan API: Successfully retrieved {Count} unreviewed loans for {Region}",
                    result.DataList.Count, region);

                return result.DataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loan API: Error fetching unreviewed loans for {Region}", region);
                return null;
            }
        }

        public async Task<List<TATOutputResponse>?> GetWithinTATUnresolvedLoansByRegion(string region)
        {
            // The specific URL from your requirement
            var urlPath = $"/api/Dashboard/data/within/unresolved/{region}";

            try
            {
                // We call the wrapper because of the $values JSON structure
                var result = await _apiService.GetAsync<TATOutputResponseWrapper>(urlPath);

                if (result == null || result.DataList == null)
                {
                    _logger.LogWarning("Loan API: No data returned for region {Region} at {Url}", region, urlPath);
                    return new List<TATOutputResponse>();
                }

                _logger.LogInformation("Loan API: Successfully retrieved {Count} unreviewed loans for {Region}",
                    result.DataList.Count, region);

                return result.DataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loan API: Error fetching unreviewed loans for {Region}", region);
                return null;
            }
        }

        public async Task<List<TATOutputResponse>?> GetBeyondTATUnresolvedLoansByRegion(string region)
        {
            // The specific URL from your requirement
            var urlPath = $"/api/Dashboard/data/beyond/unresolved/{region}";

            try
            {
                // We call the wrapper because of the $values JSON structure
                var result = await _apiService.GetAsync<TATOutputResponseWrapper>(urlPath);

                if (result == null || result.DataList == null)
                {
                    _logger.LogWarning("Loan API: No data returned for region {Region} at {Url}", region, urlPath);
                    return new List<TATOutputResponse>();
                }

                _logger.LogInformation("Loan API: Successfully retrieved {Count} unreviewed loans for {Region}",
                    result.DataList.Count, region);

                return result.DataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loan API: Error fetching unreviewed loans for {Region}", region);
                return null;
            }
        }

        public async Task<string?> GetDiscrepancyRemarks(Guid appId)
        {
            var urlPath = $"/api/Dashboard/remarks/{appId}";

            try
            {
                var result = await _apiService.GetAsync<DiscrepancyRemarkResponse>(urlPath);

                if (result == null)
                {
                    _logger.LogError("Remarks API: Received null response for AppId: {AppId}", appId);
                    return null;
                }

                if (!result.IsSuccess)
                {
                    // The API returns false if no remarks exist, which is a valid business case
                    _logger.LogInformation("Remarks API: {Message} (AppId: {AppId})", result.ResponseMessage, appId);
                    return null;
                }

                return result.ResponseMessage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Remarks API: Exception occurred while fetching remarks for {AppId}", appId);
                return null;
            }
        }

    }
}