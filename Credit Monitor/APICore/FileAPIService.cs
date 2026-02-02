using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Newtonsoft.Json; 

namespace Credit_Monitor.APICore
{
    public class FileAPIService : IFileAPIService
    {
        private readonly IBackendAPIService _apiService;
        private readonly ILogger<FileAPIService> _logger; 

        public FileAPIService(IBackendAPIService apiService, ILogger<FileAPIService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<FileDetailDto>> GetBatchFileMetaData(Guid batchId)
        {
            var urlPath = $"/api/Finacle/GetAllFilesInfo?batchId={batchId}";

            _logger.LogInformation("File API: Requesting metadata for Batch {BatchId} via {Url}", batchId, urlPath);

            try
            {
                var result = await _apiService.GetAsync<List<FileDetailDto>>(urlPath);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("File API: No files found or result was null for Batch {BatchId}", batchId);
                    return new List<FileDetailDto>();
                }

                _logger.LogInformation("File API: Successfully retrieved {Count} files for Batch {BatchId}", result.Count, batchId);

                var jsonSample = JsonConvert.SerializeObject(result, Formatting.Indented);
                _logger.LogInformation("File API: Raw Metadata Content: {Json}", jsonSample);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File API: Error occurred while fetching metadata for Batch {BatchId}", batchId);
                return new List<FileDetailDto>();
            }
        }
    }
}