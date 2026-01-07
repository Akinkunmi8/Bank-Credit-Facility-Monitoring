using Credit_Monitor.Models;

namespace Credit_Monitor.Data
{
    public class InMemoryCAT
    {
        public static List<CAT> Assessments = new()
        {
            new CAT
            {
                Id = 1,
                customerName = "DELE DELE",
                accNum = "1234567890",
                creditDate = DateTime.Parse("2026-01-06T07:37:48.435"),
                directorate = "North",
                schemeCode = "ESb3",
                accountOfficer = "Akinkunmi",
                productPaperType = "Term Loan",
                currentOutstanding = 100000000m,
                disbursementRequired = 100000000000m,
                outstandingAfterDisbursement = 100000000000m,
                approvalLimit = 100000000000m,
                interest = "29%",
                fees = "1.5%",
                approvalLevel = "ECC",
                typeOfApproval = "Standard",
                facilityType = "Individual assessed credits",
                valuedate = DateTime.Parse("2026-01-02T07:37:48.435"),
                maturityOrExpiry = DateTime.Parse("2030-01-06T07:37:48.435")
            },
            new CAT
            {
                Id = 2,
                customerName = "CHUKWUMA OKORO",
                accNum = "0987654321",
                creditDate = DateTime.Parse("2026-01-05T10:15:00"),
                directorate = "South",
                schemeCode = "SBG1",
                accountOfficer = "Babatunde",
                productPaperType = "Overdraft",
                currentOutstanding = 5000000m,
                disbursementRequired = 25000000m,
                outstandingAfterDisbursement = 30000000m,
                approvalLimit = 50000000m,
                interest = "25%",
                fees = "2.0%",
                approvalLevel = "BCC",
                typeOfApproval = "Standard",
                facilityType = "SME Credits",
                valuedate = DateTime.Parse("2026-01-01T09:00:00"),
                maturityOrExpiry = DateTime.Parse("2027-01-01T00:00:00")
            }
        };
    }
}