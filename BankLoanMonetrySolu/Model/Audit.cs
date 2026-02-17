using BankLoanMonetrySolu.Model;

namespace BankLoanMonetrySolu.Models
{
    public class Audit
    {
        public Guid Id { get; set; }                 // PK

        public Guid? AppId { get; set; }              // Loan FK
        public string UserId { get; set; }             // User FK

        public AuditAction Action { get; set; }

        public string? IpAddress { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties (optional but recommended)
        public FinacleData FinacleData { get; set; } = default!;
        public AppUsers User { get; set; } = default!;
    }

    public enum AuditAction
    {
        Approve = 1,
        Reject = 2,
        Confirm = 3,
        resolve = 4,
        Reconfirm = 4,
        CreateUser = 5,
        Login = 6

    }

}
