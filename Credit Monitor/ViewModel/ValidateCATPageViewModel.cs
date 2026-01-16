using Credit_Monitor.ViewModel;

namespace Credit_Monitor.ViewModel
{
    public class ValidateCATPageViewModel
    {
        public FinacleDataViewModel? FinacleData { get; set; }
        public CATViewModel? CatData { get; set; }
        public DiscrepanciesViewModel? Discrepancies { get; set; }
    }
    public class FinacleDataViewModel
    {
        public string? Id { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public string? FacilityType { get; set; }
        public string? SbuName { get; set; }
        public decimal SanctionLimit { get; set; }
        public decimal TotalBalanceFcy { get; set; }
        public decimal TotalBalanceLcy { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal FxRate { get; set; }
        public decimal IntRate { get; set; }
        public string? FxRateDisplay { get; set; }
        public string? IntRateDisplay { get; set; }
        public DateTime DateGranted { get; set; }
        public DateTime ExpiryDate { get; set; }
    }    
}