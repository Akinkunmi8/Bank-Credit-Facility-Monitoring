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


    public class FinacialCovenantTrackerResponse
    {
        public string? AppId { get; set; }
        public ConventFinacialDto? Financials { get; set; }
        public bool isSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class BulkFinacialCovenantTrackerResponse
    {
        public string? AppId { get; set; }
        public List<ConventFinacialDto>? uploadedDatas { get; set; }
        public bool isSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
