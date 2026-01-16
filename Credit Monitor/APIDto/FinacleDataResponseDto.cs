using System;
using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{

    public class FinacleDataResponseDto
    {
        [JsonProperty("$id")]
        public string? Id { get; set; }

        [JsonProperty("accounT_NAME")]
        public string? AccountName { get; set; }

        [JsonProperty("accounT_NUMBER")]
        public string? AccountNumber { get; set; }

        [JsonProperty("facilitY_TYPE")]
        public string? FacilityType { get; set; }

        [JsonProperty("sbU_NAME")]
        public string? SbuName { get; set; }

        [JsonProperty("sanctioN_LIMIT")]
        public decimal SanctionLimit { get; set; }

        [JsonProperty("totaL_BALANCE_FCY")]
        public decimal TotalBalanceFcy { get; set; }

        [JsonProperty("totaL_BALANCE_LCY")]
        public decimal TotalBalanceLcy { get; set; }

        [JsonProperty("totaL_BALANCE")]
        public decimal TotalBalance { get; set; }

        [JsonProperty("fX_RATE")]
        public decimal FxRate { get; set; }

        [JsonProperty("inT_RATE")]
        public decimal IntRate { get; set; }

        [JsonProperty("fX_RATE_DISPLAY")]
        public string? FxRateDisplay { get; set; }

        [JsonProperty("inT_RATE_DISPLAY")]
        public string? IntRateDisplay { get; set; }

        [JsonProperty("datE_GRANTED")]
        public DateTime DateGranted { get; set; }

        [JsonProperty("expirY_DATE")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("responseMessage")]
        public string? ResponseMessage { get; set; }
    }
}
