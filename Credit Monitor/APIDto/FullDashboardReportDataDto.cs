using Credit_Monitor.APIsDto;

namespace Credit_Monitor.APIDto
{
    public class FullDashboardReportDataDto
    {
        // Finacle Data
        public FinacleDataResponseDto? Unreviewed { get; set; }
        public FinacleDataResponseDto? Reviewed { get; set; }
        public FinacleDataResponseDto? UnResoved { get; set; }
        public FinacleDataResponseDto? Approve { get; set; }
        public FinacleDataResponseDto? Resolved { get; set; }

        // TAT Data
        public TATResponseDto? BeyondTATReview { get; set; }
        public List<FinacleRecord>? WithinTATUnreviewed { get; set; }
        public List<FinacleRecord>? BeyondTATUnreviewed { get; set; }
        public List<FinacleRecord>? WithinTATUnresolve { get; set; }
        public List<FinacleRecord>? BeyondTATUnresolve { get; set; }
    }

    public class FinacleReportResponseDto
    {
        public List<FinacleDataResponseDto>? DataList { get; set; }

        public int? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string? ResponseMessage { get; set; }
    }
}
