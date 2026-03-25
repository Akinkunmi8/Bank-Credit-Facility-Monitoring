using Credit_Monitor.APIDto;

namespace Credit_Monitor.Interfaces
{
    public interface IFileAPIService
    {
        public Task<List<FileItemDto>> GetCustomerFileMetaData(Guid appId);
        public Task<bool> UpdateFileVerification(Guid fileId, bool isVerified);
        public Task<List<FileItemDto>> EditFile(Guid appId, FileItemDto fileDto);
        public Task<Guid?> AddFile(FileItemDto fileDto);
    }
}
