namespace BankLoanMonetrySolu.DTO
{
    public class CompareCatDto
    {
        public string? CustomerName { get; set; }
        public string? AccNum { get; set; }
        public string? Directorate { get; set; }
        public string? ProductPaperType { get; set; }
        public decimal? DisbursementRequired { get; set; }
        public decimal? ApprovalLimit { get; set; }
        public string? interest { get; set; }
        public string? Fees { get; set; }
        public DateTime? Valuedate { get; set; }
        public DateTime? MaturityOrExpiry { get; set; }
        public bool? IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
    }

}
