namespace BankLoanMonetrySolu.DTO
{
    public class DiscrepancyRemarkDto
    {
        public Guid Id { get; set; } // If null, we know it's a new record
        public Guid AppId { get; set; }
        public string? Discrepancies { get; set; }
        public string? Remarks { get; set; }
        public string? RecomendedAction { get; set; }
        public string? Responsibility { get; set; }
    }
}
