using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface IFinacleData
    {
        Task<FinacleResponseDto> GetALlFincaleData();
        Task<FinacleResponseDto> GetAllPendingConfirmationByRegion(string region);
        Task<FinacleResponseDto> GetAllConfirmedFinacleData( string region);
        Task<FinacleResponseDto> GetAllReConfirmedFinacleDataByRegion(string region);
        Task<FinacleResponseDto> GetAllAprovedFinacleData(string region);
        Task<FinacleResponseDto> GetResolvedFinacleData(string region);
        Task<ServiceResponseDto> UpsertDiscrepancyRemark(DiscrepancyRemarkDto dto);
        Task<List<DiscrepancyRemarkDto>> GetRemarkByAppId(Guid appId);
        Task<int> GetTotalPendingCount(string region);
        Task<int> GetTotalConfirmedCount(string region);
        Task<int> GetTotalReConfirmedCount(string region);
        Task<int> GetTotalApproveCount(string region);
        Task<int> GetTotalResolvedCount(string region);
        Task<bool> UpdateFinacleStatus(FinacleStatusUpdateDto dto);
        Task<FinacleResponseDto> GetALLLoans();
        Task<FinacleResponseDto> GetCustormerAndLoanDetailsByAppId(string appId);
        Task<FincaleCompareDataDto> GetFinacleCompareData(string acctNum);
        //Task<Guid>SaveFilesAsync(string appId, List<IFormFile> files, LoanDocumentType fileType);
        Task<Guid> SaveFilesAsync(UploadFilesDto dto);
        Task<string> AddFile(FileViewDto file);
        Task<List<FileEditDto>> GetFileByAppIdAsync(Guid AppId);
        Task<List<FileEditDto>> EditFileByAppIdAsync(Guid appId, FileEditDto updateDto);
        Task<bool> UpdateFileStatus(Guid fileId, bool verify);
        Task<List<FileViewDto>> GetAllFilesByBatchIdAsync(Guid batchId);
        Task<string> DeleteFileByFileId(Guid appId, Guid FileId);


    }
}
