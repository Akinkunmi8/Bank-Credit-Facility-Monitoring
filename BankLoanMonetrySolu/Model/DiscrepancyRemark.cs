using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLoanMonetrySolu.Model
{
    public class DiscrepancyRemark
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // Ensures new records get a Guid automatically
        public Guid AppId { get; set; } // No longer the Key, just a Foreign Key

        public string? Discrepancies { get; set; }
        public string? Remarks { get; set; }
        public string? RecomendedAction { get; set; }
        public string? Responsibility { get; set; }

        // Matches the SQL column
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("AppId")]
        public virtual FinacleData FinacleData { get; set; }
    }
}
