namespace Credit_Monitor.ViewModel
{
    public class CATViewModel
    {
        public string? CustomerName { get; set; }
        public string? AccNum { get; set; }
        public DateTime CreditDate { get; set; }
        public string? Directorate { get; set; }
        public string? SchemeCode { get; set; }
        public string? AccountOfficer { get; set; }
        public string? ProductPaperType { get; set; }
        public decimal CurrentOutstanding { get; set; }
        public decimal DisbursementRequired { get; set; }
        public decimal OutstandingAfterDisbursement { get; set; }
        public decimal ApprovalLimit { get; set; }
        public decimal Interest { get; set; }
        public decimal Fees { get; set; }
        public string? ApprovalLevel { get; set; }
        public string? TypeOfApproval { get; set; }
        public string? FacilityType { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime MaturityOrExpiry { get; set; }
    }
}