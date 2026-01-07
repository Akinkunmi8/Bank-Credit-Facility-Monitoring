using BankLoanMonetrySolu.Model;

namespace BankLoanMonetrySolu.DTO
{
    public class FinacleResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
        public FinacleDataDto? Data { get; set; }
        public List<FinacleDataDto>? DataList { get; set; }
        public List<CustomerDetailsDto>? detailsDtos { get; set; }
        public CustomerDetailsDto? detailsDto { get; set; }
    }
}
