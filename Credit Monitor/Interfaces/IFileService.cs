using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface IFileService
    {
        Task<List<DocumentDetailsViewModel>> GetCustomerDocumentsAsync(Guid batchId);
        Task<Guid?> UploadDocumentAsync(FileItemDto fileDto);
        Task<List<DocumentDetailsViewModel>> UpdateDocumentAsync(Guid appId, FileItemDto fileDto);
        Task<bool> ToggleVerificationAsync(Guid fileId, bool status);
        Task<bool> DeleteDocumentAsync(Guid appId, Guid fileId);
    }
}
