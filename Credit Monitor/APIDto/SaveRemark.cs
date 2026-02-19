namespace Credit_Monitor.APIDto
{
    public class SaveRemarkRequestDto
    {
        public Guid AppId { get; set; }
        public string Discrepancies { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;

        // Kept with one 'm' to match your request snippet
        public string RecomendedAction { get; set; } = string.Empty;
        public string Responsibility { get; set; } = string.Empty;
    }

    public class SaveRemarkResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
    }
}
