using Credit_Monitor.ViewModel;

namespace Credit_Monitor.ViewModel
{
    public class CreditMonitoringReportViewModel
    {
        public PendingLoanWithCustomerDetailsViewModel CreditProfile { get; set; }
            = new PendingLoanWithCustomerDetailsViewModel();

        public List<DocumentDetailsViewModel> Documents { get; set; }
            = new List<DocumentDetailsViewModel>();

        public CovenantViewModel Covenants { get; set; }
            = new CovenantViewModel();

        public DateTime GeneratedAt { get; set; }

        public string? GeneratedBy { get; set; }
    }
}
