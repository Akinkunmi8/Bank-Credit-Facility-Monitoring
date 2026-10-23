namespace Credit_Monitor.ViewModel
{
    public class ResolutionDecisionViewModel
    {
        public Guid ApplicationId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string Decision { get; set; } = string.Empty;

        public DateTime? Deadline { get; set; }
    }
}
