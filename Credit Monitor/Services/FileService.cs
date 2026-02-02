using AutoMapper;
using Credit_Monitor.APICore;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Services
{
    public class FileService : IFileService
    {
        private readonly IFileAPIService _fileAPIService;
        private readonly IMapper _mapper;
        private readonly ILogger<FileService> _logger; // 1. Add Logger

        public FileService(IFileAPIService fileAPIService, IMapper mapper, ILogger<FileService> logger)
        {
            _fileAPIService = fileAPIService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DocumentDetailsViewModel>> GetCustomerDocumentsAsync(Guid batchId)
        {
            _logger.LogInformation("Requesting document metadata for BatchId: {BatchId}", batchId);

            try
            {
                var fileDtos = await _fileAPIService.GetBatchFileMetaData(batchId);

                if (fileDtos == null || !fileDtos.Any())
                {
                    // 2. Log as a warning because a loan batch without documents might be a processing error
                    _logger.LogWarning("No document metadata found for BatchId: {BatchId}", batchId);
                    return new List<DocumentDetailsViewModel>();
                }

                _logger.LogInformation("Found {Count} documents for BatchId: {BatchId}. Proceeding with mapping.", fileDtos.Count(), batchId);

                var viewModels = _mapper.Map<List<DocumentDetailsViewModel>>(fileDtos);

                // Explicitly set the BatchId for each document
                foreach (var doc in viewModels)
                {
                    doc.BatchId = batchId;
                }

                _logger.LogInformation("Successfully mapped {Count} documents for BatchId: {BatchId}", viewModels.Count, batchId);
                return viewModels;
            }
            catch (Exception ex)
            {
                // 3. Log critical failures (e.g., File Server is down)
                _logger.LogError(ex, "Failed to retrieve customer documents for BatchId: {BatchId}", batchId);
                throw;
            }
        }
    }
}