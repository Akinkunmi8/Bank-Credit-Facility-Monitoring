using BankLoanMonetrySolu.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankLoanMonetrySolu.Model
{
    public class LoanFileItem
    {
        public Guid Id { get; set; }
         public Guid FileBatchId { get; set; }
        // This is your Foreign Key
        public Guid AppId { get; set; }

        // Navigation Property: This links the actual FinacleData object
        [ForeignKey("AppId")]
        public virtual FinacleData FinacleRecord { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileFormat { get; set; } = null!;
        public LoanDocumentType DocumentType { get; set; }
        public bool IsFileVerified { get; set; } = false;

        public bool IsFileExitOnServer { get; set; } = false;

        public string? CustomDocumentType { get; set; }
        public string? Region { get; set; }
        public string?RequesterUsername { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public LoanFileBatch FileBatch { get; set; } = null!;
    }

}
