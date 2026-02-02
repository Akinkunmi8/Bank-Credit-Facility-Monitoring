using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json; // 1. Ensure Newtonsoft is imported

namespace Credit_Monitor.APICore
{
    public class CatAPIService : ICatAPIService
    {
        private readonly IBackendAPIService _apiService;
        private readonly ILogger<CatAPIService> _logger;

        public CatAPIService(IBackendAPIService apiService, ILogger<CatAPIService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<CatDataDto?> GetCustomerLoanDetailsfromCAT(string AccNum)
        {
            var urlPath = $"/api/CAT/GetByAccount/{AccNum}";
            _logger.LogInformation("CAT API Request: Outgoing GET to {UrlPath}", urlPath);

            try
            {
                var result = await _apiService.GetAsync<CatResponseDto>(urlPath);

                if (result == null)
                {
                    _logger.LogWarning("CAT API Response: Null object returned for Account {AccNum}", AccNum);
                    return null;
                }

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("CAT API Response: Failure for Account {AccNum}. Message: {Message}",
                        AccNum, result.ResponseMessage ?? "No error message provided");
                    return null;
                }

                // 2. Serialize the Data object to see exactly what came back from the API
                var jsonData = JsonConvert.SerializeObject(result.Data, Formatting.Indented);

                _logger.LogInformation("CAT API Response: Success for Account {AccNum}. Data Received: {Data}",
                    AccNum, jsonData);

                return result.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CAT API Error: Critical exception while calling {UrlPath}", urlPath);
                return null;
            }
        }
    }
}