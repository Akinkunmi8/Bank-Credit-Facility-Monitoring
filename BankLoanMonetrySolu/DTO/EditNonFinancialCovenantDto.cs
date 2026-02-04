namespace BankLoanMonetrySolu.DTO
{
    public class EditNonFinancialCovenantDto
    {
        public int Id { get; set; } // Required to find the specific record
        public string? CovenantType { get; set; }
        public string? Description { get; set; }
        public FrequencyEnum? Frequency { get; set; }
    }

    public class AddNonFincialConvDto
    {

        public string? CovenantType { get; set; }
        public string? Description { get; set; }
        public FrequencyEnum? Frequency { get; set; }
    }
}
