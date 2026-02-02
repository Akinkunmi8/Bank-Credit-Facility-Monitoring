namespace BankLoanMonetrySolu.DTO
{
    public class NonFinacialConventDto
    {
        public int Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? CovenantType { get; set; }
        public string? Description { get; set; }
        public FrequencyEnum Frequency { get; set; }

    }
}
