using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.Model
{
    public class LoanFileItem
    {
        public Guid Id { get; set; }
        public Guid FileBatchId { get; set; }

        public string FileName { get; set; } = null!;
        public string FileFormat { get; set; } = null!;
        public LoanDocumentType DocumentType { get; set; }
        public bool IsFileVerified { get; set; } = false;
        public string? CustomDocumentType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public LoanFileBatch FileBatch { get; set; } = null!;
    }

}
