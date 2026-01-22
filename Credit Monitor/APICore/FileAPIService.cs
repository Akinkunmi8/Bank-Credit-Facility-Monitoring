using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;

namespace Credit_Monitor.APICore
{
    public class FileAPIService : IFileAPIService
    {
        private readonly IBackendAPIService _apiService;

        public FileAPIService(IBackendAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<FileDetailDto>> GetBatchFileMetaData(Guid batchId)
        {
            var urlPath = $"/api/Finacle/GetAllFilesInfo?batchId={batchId}";

            // Map directly to a List. Newtonsoft handles the "$values" unwrapping automatically
            var result = await _apiService.GetAsync<List<FileDetailDto>>(urlPath);

            // If result is null, return empty list; otherwise return the result
            return result ?? new List<FileDetailDto>();
        }
    }
}