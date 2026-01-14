namespace Credit_Monitor.APIsDto
{
    public class CatCompareData
    {
        public string customerName { get; set; }
        public string accNum { get; set; }
        public string directorate { get; set; }
        public string productPaperType { get; set; }

        // Large numbers are best handled as decimals for precision
        public decimal disbursementRequired { get; set; }
        public decimal approvalLimit { get; set; }

        // Kept as strings since they are quoted in the JSON
        public string interest { get; set; }
        public string fees { get; set; }

        public DateTime valuedate { get; set; }
        public DateTime maturityOrExpiry { get; set; }

        public bool isSuccess { get; set; }
        public string responseMessage { get; set; }
    }
}
