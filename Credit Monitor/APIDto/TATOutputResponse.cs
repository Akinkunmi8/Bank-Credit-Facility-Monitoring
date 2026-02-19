using Newtonsoft.Json;
namespace Credit_Monitor.APIDto
{

    public class TATOutputResponseWrapper
    {
        // Newtonsoft uses $values to wrap lists when ReferenceLoopHandling is on
        [JsonProperty("$values")]
        public List<TATOutputResponse> DataList { get; set; } = new();
    }

    public class TATOutputResponse
    {
        public Guid Id { get; set; }
        public string Country { get; set; }

        [JsonProperty("lE_BOOK")]
        public string LeBook { get; set; }

        [JsonProperty("yeaR_MONTH")]
        public string YearMonth { get; set; }

        [JsonProperty("risK_ASSET_TYPE")]
        public string RiskAssetType { get; set; }

        [JsonProperty("contracT_ID")]
        public string ContractId { get; set; }

        [JsonProperty("accounT_NUMBER")]
        public string AccountNumber { get; set; }

        [JsonProperty("accounT_NAME")]
        public string AccountName { get; set; }

        [JsonProperty("facilitY_TYPE")]
        public string FacilityType { get; set; }

        [JsonProperty("loaN_BALANCE_LCY")]
        public decimal LoanBalanceLcy { get; set; }

        [JsonProperty("totaL_BALANCE_LCY")]
        public decimal TotalBalanceLcy { get; set; }

        [JsonProperty("inT_RATE")]
        public double InterestRate { get; set; }

        [JsonProperty("dayS_PAST_DUE")]
        public int DaysPastDue { get; set; }

        [JsonProperty("datE_GRANTED")]
        public DateTime? DateGranted { get; set; }

        [JsonProperty("expirY_DATE")]
        public DateTime? ExpiryDate { get; set; }

        [JsonProperty("sanctioN_LIMIT")]
        public decimal SanctionLimit { get; set; }

        [JsonProperty("cusT_SECTOR_DESC")]
        public string CustSectorDesc { get; set; }

        [JsonProperty("cusT_SUB_SECTOR_DESC")]
        public string CustSubSectorDesc { get; set; }

        public decimal Instalment { get; set; }

        [JsonProperty("priN_REPAYMENT_FREQUENCY")]
        public string PrincipalRepaymentFrequency { get; set; }

        [JsonProperty("collateraL_DESCRIPTION")]
        public string CollateralDescription { get; set; }

        [JsonProperty("brancH_NAME")]
        public string BranchName { get; set; }

        [JsonProperty("accounT_OFFICER")]
        public string AccountOfficer { get; set; }

        public string RiskRating { get; set; }

        public bool Confirmed { get; set; }
        public bool Approved { get; set; }
        public bool IsResolved { get; set; }

        public DateTime ProcessedDate { get; set; }
        public Guid LoanFileBatchId { get; set; }
    }
}
