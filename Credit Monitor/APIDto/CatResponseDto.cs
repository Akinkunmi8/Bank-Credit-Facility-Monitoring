using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    public class CatResponseDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        [JsonProperty("data")]
        public CatDataDto Data { get; set; }

        [JsonProperty("dataList")]
        public object DataList { get; set; }
    }

    public class CatDataDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("accNum")]
        public string AccNum { get; set; }

        [JsonProperty("creditDate")]
        public DateTime CreditDate { get; set; }

        [JsonProperty("directorate")]
        public string Directorate { get; set; }

        [JsonProperty("schemeCode")]
        public string SchemeCode { get; set; }

        [JsonProperty("accountOfficer")]
        public string AccountOfficer { get; set; }

        [JsonProperty("productPaperType")]
        public string ProductPaperType { get; set; }

        [JsonProperty("currentOutstanding")]
        public decimal CurrentOutstanding { get; set; }

        [JsonProperty("disbursementRequired")]
        public decimal DisbursementRequired { get; set; }

        [JsonProperty("outstandingAfterDisbursement")]
        public decimal OutstandingAfterDisbursement { get; set; }

        [JsonProperty("approvalLimit")]
        public decimal ApprovalLimit { get; set; }

        [JsonProperty("interest")]
        public decimal Interest { get; set; }

        [JsonProperty("fees")]
        public decimal Fees { get; set; }

        [JsonProperty("approvalLevel")]
        public string? ApprovalLevel { get; set; }

        [JsonProperty("typeOfApproval")]
        public string? TypeOfApproval { get; set; }

        [JsonProperty("facilityType")]
        public string? FacilityType { get; set; }

        [JsonProperty("valuedate")]
        public DateTime ValueDate { get; set; }

        [JsonProperty("maturityOrExpiry")]
        public DateTime MaturityOrExpiry { get; set; }
    }
}
