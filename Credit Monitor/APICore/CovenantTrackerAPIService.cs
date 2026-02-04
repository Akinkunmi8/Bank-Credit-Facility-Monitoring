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

        public async Task<UpdateCovenantResponseDto?> UpdateNonFinancialCovenant(Guid appId, UpdateCovenantRequestDto request)
        {
            // The GUID goes in the URL path per your provided example
            var urlPath = $"/api/ConventTracker/EditNonFinancial/{appId}";

            _logger.LogInformation("Covenant API Request: Updating non-financial covenant for AppId {AppId}", appId);

            try
            {
                // Using PostAsync. The 'request' object will be serialized to JSON via Newtonsoft
                var result = await _apiService.PutAsync<UpdateCovenantResponseDto>(request, urlPath);

                if (result == null)
                {
                    _logger.LogWarning("Covenant API Response: Null response received for update on AppId {AppId}", appId);
                    return null;
                }

                _logger.LogInformation("Covenant API Response: Update result for {AppId}: {Message}",
                    appId, result.Message);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Covenant API Error: Failed to update covenant at {UrlPath}", urlPath);
                return new UpdateCovenantResponseDto { Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<UpdateCovenantResponseDto?> AddNonFinancialCovenant(Guid appId, UpdateCovenantRequestDto request)
        {
            var urlPath = $"/api/ConventTracker/add-covenant/{appId}";

            _logger.LogInformation("Covenant API Request: Adding new covenant for AppId {AppId}", appId);

            try
            {
                var result = await _apiService.PostAsync<UpdateCovenantResponseDto>(request, urlPath);

                if (result == null)
                {
                    _logger.LogWarning("Covenant API Response: Null response for add on AppId {AppId}", appId);
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Covenant API Error: Failed to add covenant at {UrlPath}", urlPath);
                return new UpdateCovenantResponseDto { Message = $"Connection Error: {ex.Message}" };
            }
        }
    }
}