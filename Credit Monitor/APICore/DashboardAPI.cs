using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json; // Import Newtonsoft for JSON serialization
using Microsoft.Extensions.Logging;

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
    }
}