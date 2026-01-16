namespace BankLoanMonetrySolu.Model
{
    public class LoanFileBatch
    {
        public Guid Id { get; set; }
        public string AppId { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LoanFileItem> Files { get; set; } = new List<LoanFileItem>();
    }

}
