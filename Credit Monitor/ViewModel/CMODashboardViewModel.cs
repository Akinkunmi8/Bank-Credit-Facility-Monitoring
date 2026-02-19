namespace Credit_Monitor.ViewModel
{
    public class CMODashboardViewModel
    {
        public IEnumerable<PendingConfirmedLoanViewModel>? loans { get; set; }
        public int TotalReviewedLoans { get; set; }
        public int TotalUnreviewedLoans { get; set; }
        public int TotalUnresolvedLoans { get; set; }
        public int TotalResolvedLoans { get; set; }
        public int WithinTATUnreviewed { get; set; }
        public int WithinTATUnresolved { get; set; }
        public int BeyondTATUnreviewed { get; set; }
        public int BeyondTATUnresolved { get; set; }


        // --- ADD THESE FOR PAGINATION & SEARCH ---
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string? SearchTerm { get; set; }
    }
}
