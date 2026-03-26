namespace BankLoanMonetrySolu.DTO
{
    public class FullDashboardReportData
    {
        // Finacle Data
        public FinacleResponseDto? Unreviewed { get; set; }
        public FinacleResponseDto? Reviewed { get; set; }
        public FinacleResponseDto? UnResoved { get; set; }
        public FinacleResponseDto? Approve { get; set; }
        public FinacleResponseDto? Resolved { get; set; }

        // TAT Data
        public TATResponseDto? BeyondTATReview { get; set; }
        public List<FinacleDataDto>? WithinTATUnreviewed { get; set; }
        public List<FinacleDataDto>? BeyondTATUnreviewed { get; set; }
        public List<FinacleDataDto>? WithinTATUnresolve { get; set; }
        public List<FinacleDataDto>? BeyondTATUnresolve { get; set; }
    }
}
