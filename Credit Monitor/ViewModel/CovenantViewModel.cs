namespace Credit_Monitor.ViewModel
{
    public class CovenantViewModel
    {
        public Guid AppId { get; set; }
        public string AccountNumber { get; set; }
        public List<FinancialCovenantVM> Financials { get; set; } = new();
        public List<NonFinancialCovenantVM> NonFinancials { get; set; } = new();
    }

    public class FinancialCovenantVM
    {
        public int Id { get; set; }
        public string CovenantType { get; set; }
        public string Description { get; set; }
        public decimal TargetValue { get; set; }
        public FrequencyEnum Frequency { get; set; } // The Enum type
        public string DataSource { get; set; }
    }

    public class NonFinancialCovenantVM
    {
        public int Id { get; set; }
        public string CovenantType { get; set; }
        public string Description { get; set; }
        public FrequencyEnum Frequency { get; set; } // The Enum type
    }

    public enum FrequencyEnum
    {
        Weekly,
        Monthly,
        Quarterly,
        BiAnnually,
        Annually
    }
}