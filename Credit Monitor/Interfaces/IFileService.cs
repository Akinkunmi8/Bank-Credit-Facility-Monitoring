using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface IFileService
    {
        Task<List<DocumentDetailsViewModel>> GetCustomerDocumentsAsync(Guid batchId);
    }
}
