namespace Credit_Monitor.ViewModel
{
    public class CMSDashboardViewModel
    {
        public IEnumerable<PendingConfirmedLoanViewModel>? Confirmedloans { get; set; }
        public int TotalApprovedLoans { get; set; }
        public int TotalReviewedLoans { get; set; }
        public int BeyondTATReviewed { get; set; }
        public int BeyondTATUnreviewed { get; set; }

        // --- ADD THESE FOR PAGINATION & SEARCH ---
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string? SearchTerm { get; set; }
    }
}
