using BankLoanMonetrySolu.Model;

namespace BankLoanMonetrySolu.DTO
{
    public class CATResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
        public CreditAvailmentTicketDto? Data { get; set; }
        public List<CreditAvailmentTicketDto>? DataList { get; set; }
    }

}
