using System.Text.Json.Serialization;

namespace Credit_Monitor.APIsDto
{
    public class CATResponse
    {
        public bool isSuccess { get; set; }
        public string responseMessage { get; set; }
        public object data { get; set; } // null in your example
        public DataListContainer dataList { get; set; }
    }

    public class DataListContainer
    {
        // This property maps to the "$values" array in your JSON
        [JsonPropertyName("$values")]
        public List<CatRecord> values { get; set; }
    }

    public class CatRecord
    {
        public string customerName { get; set; }
        public string accNum { get; set; }
        public DateTime creditDate { get; set; }
        public string directorate { get; set; }
        public string schemeCode { get; set; }
        public string accountOfficer { get; set; }
        public string productPaperType { get; set; }
        public decimal currentOutstanding { get; set; }
        public decimal disbursementRequired { get; set; }
        public decimal outstandingAfterDisbursement { get; set; }
        public decimal approvalLimit { get; set; }
        public string interest { get; set; }
        public string fees { get; set; }
        public string approvalLevel { get; set; }
        public string typeOfApproval { get; set; }
        public string facilityType { get; set; }
        public DateTime valuedate { get; set; }
        public DateTime maturityOrExpiry { get; set; }
    }
}
