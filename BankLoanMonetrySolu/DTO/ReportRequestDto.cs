namespace BankLoanMonetrySolu.DTO
{
    public class ReportRequestDto
    {
        public string Role { get; set; } // CMO, CMS, AO, BM
        public string Region { get; set; }
        public string Format { get; set; } // Excel or PDF
    }
}
