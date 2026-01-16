namespace Credit_Monitor.ViewModel
{
    public class StaffDashboardViewModel
    {
        public IEnumerable<PendingConfirmedLoanViewModel>? loans { get; set; }
        public int TotalPendingLoans { get; set; }
        public int TotalConfirmedLoans { get; set; }
        public int TotalRejectedLoans { get; set; }
        public int WithinTAT { get; set; }
        public int BeyondTAT { get; set; }

        // --- ADD THESE FOR PAGINATION & SEARCH ---
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string? SearchTerm { get; set; }
    }
}
