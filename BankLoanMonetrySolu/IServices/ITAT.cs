using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface ITAT
    {
        Task<TATResponseDto> GetWithinTAT(string region);
        Task<TATResponseDto> GetBeyondTAT(string region);
        Task<List<Guid>> GetBeyondTATAppIds(string region);
    }
}
