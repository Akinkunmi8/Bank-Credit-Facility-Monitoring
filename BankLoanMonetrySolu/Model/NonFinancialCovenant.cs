using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.Model
{
    public class NonFinancialCovenant
    {
        public int Id { get; set; }
        public Guid AppId { get; set; } // Foreign Key to FinacleData
        public string AccountNumber { get; set; } // Foreign Key to FinacleData
        public string CovenantType { get; set; }
        public string Description { get; set; }
        public FrequencyEnum Frequency { get; set; }
    }
}
