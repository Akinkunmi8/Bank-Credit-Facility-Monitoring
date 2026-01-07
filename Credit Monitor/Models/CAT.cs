using System.ComponentModel.DataAnnotations;

namespace Credit_Monitor.Models
{
    public class CAT
    {
        [Key]
        public int Id { get; set; } // Added for database primary key support

        public string? customerName { get; set; }

        public string? accNum { get; set; }

        public DateTime? creditDate { get; set; }

        public string? directorate { get; set; }

        public string? schemeCode { get; set; }

        public string? accountOfficer { get; set; }

        public string? productPaperType { get; set; }

        // Financials - Using decimal for high-precision currency values
        public decimal currentOutstanding { get; set; }

        public decimal disbursementRequired { get; set; }

        public decimal outstandingAfterDisbursement { get; set; }

        public decimal approvalLimit { get; set; }

        // Stored as strings to preserve the '%' symbol from your JSON
        public string? interest { get; set; }

        public string? fees { get; set; }

        public string? approvalLevel { get; set; }

        public string? typeOfApproval { get; set; }

        public string? facilityType { get; set; }

        public DateTime? valuedate { get; set; }

        public DateTime? maturityOrExpiry { get; set; }
    }
}