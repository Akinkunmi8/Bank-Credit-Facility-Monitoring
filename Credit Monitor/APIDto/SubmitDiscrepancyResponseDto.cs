namespace Credit_Monitor.APIDto
{
    public class SubmitDiscrepancyResponseDto
    {
        public Guid? Id { get; set; }

        public Guid AppId { get; set; }

        public string ResolutionAction { get; set; } = string.Empty;

        public string? ResolutionRemarks { get; set; }
    }
    public class SubmitResolutionForReviewDto
    {
        public Guid AppId { get; set; }
    }

    public class ServiceResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
    }
}
