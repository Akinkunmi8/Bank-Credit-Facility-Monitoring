using BankLoanMonetrySolu.DTO;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.IServices
{
    public interface ITAT
    {
        Task<TATResponseDto> GetWithinTATForUnReview(string region);
        Task<TATResponseDto> GetWithinTATForUnResolved(string region);
        Task<TATResponseDto> GetBeyondTATForUnreview(string region);
        Task<TATResponseDto> GetBeyondTATForUnResolved(string region);
        Task<TATResponseDto> GetBeyondTATForReviewedforApproval(string region);
        Task<List<Guid>> GetBeyondTATAppIds(string region);
        Task<List<FinacleDataDto>> GetDataWithinTATForUnReview(string region);
        Task<List<FinacleDataDto>> GetDataBeyondTATForUnReview(string region);
        Task<List<UnresolvedTATDetailDto>> GetDataWithinTATForUnResolved(string region);
        Task<List<UnresolvedTATDetailDto>> GetDataBeyondTATForUnResolved(string region);
    }
}
