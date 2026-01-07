using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface ITAT
    {
        Task<TATResponseDto> GetWithinTAT();
        Task<TATResponseDto> GetBeyondTAT();
    }
}
