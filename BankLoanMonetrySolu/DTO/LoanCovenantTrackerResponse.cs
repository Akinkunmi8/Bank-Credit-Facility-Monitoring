namespace BankLoanMonetrySolu.DTO
{
    public class LoanCovenantTrackerResponse
    {
        public string? AppId { get; set; }
        public string? AccountNumber { get; set; }
        public List<ConventFinacialDto>? Financials { get; set; }
        public List<NonFinacialConventDto>? NonFinancials { get; set; }
        public bool isSuccess { get; set; }
        public string? Message { get; set; }
    }
}
