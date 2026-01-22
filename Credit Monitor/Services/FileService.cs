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

        public FileService(IFileAPIService fileAPIService, IMapper mapper)
        {
            _fileAPIService = fileAPIService;
            _mapper = mapper;
        }

        public async Task<List<DocumentDetailsViewModel>> GetCustomerDocumentsAsync(Guid batchId)
        {
            var fileDtos = await _fileAPIService.GetBatchFileMetaData(batchId);

            if (fileDtos == null || !fileDtos.Any())
            {
                return new List<DocumentDetailsViewModel>();
            }

            var viewModels = _mapper.Map<List<DocumentDetailsViewModel>>(fileDtos);

            // Explicitly set the BatchId for each document
            // Since the API metadata doesn't include the BatchId in the inner loop, 
            // we attach it here so the 'View' button knows which file to open.
            foreach (var doc in viewModels)
            {
                doc.BatchId = batchId;
            }

            return viewModels;
        }
    }
}
