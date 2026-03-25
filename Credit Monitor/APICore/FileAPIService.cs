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

        public async Task<List<FileItemDto>> GetCustomerFileMetaData(Guid appId)
        {
            var urlPath = $"/api/Dashboard/get-by-app/{appId}";

            try
            {
                var result = await _apiService.GetAsync<FileResponseDto>(urlPath);

                // Check 'Values' directly now
                if (result == null || result.Values == null || !result.Values.Any())
                {
                    _logger.LogWarning("File API: No files found or result was null for AppId {appId}", appId);
                    return new List<FileItemDto>();
                }

                return result.Values;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File API: Error occurred for AppId {AppId}", appId);
                return new List<FileItemDto>();
            }
        }

        public async Task<Guid?> AddFile(FileItemDto fileDto)
        {
            var urlPath = "/api/Dashboard/add-file";
            try
            {
                var result = await _apiService.PostAsync<SingleFileResponseDto>(fileDto, urlPath);
                if (result != null && result.Success)
                {
                    return result.Data.Id;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File API: Error adding file {FileName}", fileDto.FileName);
                return null;
            }
        }

        public async Task<List<FileItemDto>> EditFile(Guid appId, FileItemDto fileDto)
        {
            var urlPath = $"/api/Dashboard/edit-file/{appId}";
            try
            {
                // The edit endpoint returns the updated list of files
                var result = await _apiService.PutAsync<FileResponseDto>(fileDto, urlPath);
                return result?.Values ?? new List<FileItemDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File API: Error editing file {FileId}", fileDto.Id);
                return new List<FileItemDto>();
            }
        }

        public async Task<bool> UpdateFileVerification(Guid fileId, bool isVerified)
        {
            var urlPath = "/api/Dashboard/update-file-verify";
            var payload = new VerifyFileRequest { FileId = fileId, IsVerified = isVerified };

            try
            {
                // Based on your example, this might be a POST or GET with body. 
                // If it's a GET with body, ensure your _apiService supports it.
                var result = await _apiService.PostAsync<FileResponseDto>(payload, urlPath);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File API: Error updating verification for {FileId}", fileId);
                return false;
            }
        }
    }
}