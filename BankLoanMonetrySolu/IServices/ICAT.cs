using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface ICAT
    {
        Task<CATResponseDto> CreateCatAsync(CreditAvailmentTicketDto cat);
        Task<CATResponseDto> GetAllCAT();
        Task<CATResponseDto> GetCATByAccountNum(string accountNum);
        Task<CompareCatDto> GetCompareCatdata(string accountNum);
    }
}
