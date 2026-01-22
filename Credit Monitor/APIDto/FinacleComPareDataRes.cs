namespace Credit_Monitor.APIsDto
{
    public class FinacleComPareDataRes
    {
        // Note: $id is handled by the serializer metadata

        public string accounT_NAME { get; set; }
        public string accounT_NUMBER { get; set; }
        public string facilitY_TYPE { get; set; }
        public string sbU_NAME { get; set; }

        // Financial decimals for calculation accuracy
        public decimal sanctioN_LIMIT { get; set; }
        public decimal totaL_BALANCE_FCY { get; set; }
        public decimal totaL_BALANCE_LCY { get; set; }
        public decimal totaL_BALANCE { get; set; }
        public decimal fX_RATE { get; set; }
        public decimal inT_RATE { get; set; }

        // Display strings for the UI
        public string fX_RATE_DISPLAY { get; set; }
        public string inT_RATE_DISPLAY { get; set; }

        // Dates
        public DateTime datE_GRANTED { get; set; }
        public DateTime expirY_DATE { get; set; }

        // Status properties
        public bool isSuccess { get; set; }
        public string responseMessage { get; set; }
    }
}
