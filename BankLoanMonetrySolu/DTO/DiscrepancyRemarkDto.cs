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
        // AO resolved at
        public string? ResolutionAction { get; set; }
        public string? ResolutionRemarks { get; set; }
        public string? RespondedBy { get; set; }
        public DateTime? RespondedAt { get; set; }
    }
}
