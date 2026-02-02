namespace BankLoanMonetrySolu.DTO
{
    public class ConventFinacialDto
    {
        public int Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? CovenantType { get; set; }
        public string? Description { get; set; }
        public decimal? TargetValue { get; set; }
        public FrequencyEnum Frequency { get; set; }
        public string DataSource { get; set; }
    }
}
