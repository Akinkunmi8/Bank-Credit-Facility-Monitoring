namespace BankLoanMonetrySolu.DTO
{
    public class UnresolvedTATDetailDto : FinacleDataDto
    {
        public DateTime? Deadline { get; set; }
        public string? Discrepancies { get; set; }
        public string? Remarks { get; set; }
        public string? RecomendedAction { get; set; }
        public string? Responsibility { get; set; }
    }

    public class ServiceResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
    }
}
