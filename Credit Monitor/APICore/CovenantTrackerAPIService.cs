using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Credit_Monitor.APICore
{
    public class CovenantTrackerAPIService : ICovenantTrackerAPIService
    {
        private readonly IBackendAPIService _apiService;
        private readonly ILogger<CovenantTrackerAPIService> _logger;

        public CovenantTrackerAPIService(IBackendAPIService apiService, ILogger<CovenantTrackerAPIService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<CovenantResponseDto?> GetCovenantDetails(Guid appId)
        {
            // Using the backend's specific route spelling: /api/ConventTracker/GetTracker/
            var urlPath = $"/api/ConventTracker/GetTracker/{appId}";

            _logger.LogInformation("Covenant API Request: Fetching details for AppId {AppId}", appId);

            try
            {
                var result = await _apiService.GetAsync<CovenantResponseDto>(urlPath);

                if (result == null)
                {
                    _logger.LogWarning("Covenant API Response: Null object returned for AppId {AppId}", appId);
                    return null;
                }

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Covenant API Response: Failure for AppId {AppId}. Message: {Message}",
                        appId, result.Message ?? "No error message provided");
                    return result; // Returning the result so the UI can show the specific failure message
                }

                // Serialize the full response to verify the $values unwrapping in your logs
                var jsonData = JsonConvert.SerializeObject(result, Formatting.Indented);

                _logger.LogInformation("Covenant API Response: Success for AppId {AppId}. Data: {Data}",
                    appId, jsonData);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Covenant API Error: Critical exception while calling {UrlPath}", urlPath);
                return null;
            }
        }
    }
}