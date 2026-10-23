using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLoanMonetrySolu.Model
{
    public class DiscrepancyRemark
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AppId { get; set; }

        public string? Discrepancies { get; set; }
        public string? Remarks { get; set; }
        public string? RecomendedAction { get; set; }
        public string? Responsibility { get; set; }

        // Matches the SQL column
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // RM/AO response
        public string? ResolutionAction { get; set; }
        public string? ResolutionRemarks { get; set; }
        public string? RespondedBy { get; set; }
        public DateTime? RespondedAt { get; set; }


        // Navigation Property
        [ForeignKey("AppId")]
        public virtual FinacleData FinacleData { get; set; }
    }
}
