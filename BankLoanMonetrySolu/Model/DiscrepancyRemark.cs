using System.ComponentModel.DataAnnotations;

namespace BankLoanMonetrySolu.Model
{
    public class DiscrepancyRemark
    {
        [Key]
        public Guid AppId { get; set; }

        public string? Discrepancies { get; set; }
        public string? Remarks { get; set; }
        public string? RecomendedAction { get; set; }
        public string? Responsibility { get; set; }

        // Navigation Property
        public virtual FinacleData FinacleData { get; set; }
    }
}
