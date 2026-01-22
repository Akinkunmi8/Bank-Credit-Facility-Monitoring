using Credit_Monitor.APIDto;

namespace Credit_Monitor.Interfaces
{
    public interface IFileAPIService
    {
        public Task<List<FileDetailDto>> GetBatchFileMetaData(Guid batchId);
    }
}
