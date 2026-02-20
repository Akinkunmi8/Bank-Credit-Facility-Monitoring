using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Credit_Monitor.APIDto
{
    public class TATOutputResponseWrapper
    {
        // Newtonsoft uses $values when PreserveReferencesHandling is enabled
        [JsonProperty("$values")]
        public List<TATOutputResponse> DataList { get; set; }
    }

    public class TATOutputResponse
    {
        public Guid id { get; set; }
        public string country { get; set; }

        [JsonProperty("lE_BOOK")]
        public string lE_BOOK { get; set; }

        [JsonProperty("yeaR_MONTH")]
        public string yeaR_MONTH { get; set; }

        [JsonProperty("risK_ASSET_TYPE")]
        public string risK_ASSET_TYPE { get; set; }

        [JsonProperty("contracT_ID")]
        public string contracT_ID { get; set; }

        [JsonProperty("accounT_NUMBER")]
        public string accounT_NUMBER { get; set; }

        [JsonProperty("accounT_NAME")]
        public string accounT_NAME { get; set; }

        [JsonProperty("facilitY_TYPE")]
        public string facilitY_TYPE { get; set; }

        [JsonProperty("loaN_BALANCE_LCY")]
        public decimal loaN_BALANCE_LCY { get; set; }

        [JsonProperty("totaL_BALANCE_LCY")]
        public decimal totaL_BALANCE_LCY { get; set; }

        [JsonProperty("inT_RATE")]
        public double inT_RATE { get; set; }

        [JsonProperty("dayS_PAST_DUE")]
        public int dayS_PAST_DUE { get; set; }

        [JsonProperty("datE_GRANTED")]
        public DateTime? datE_GRANTED { get; set; }

        [JsonProperty("expirY_DATE")]
        public DateTime? expirY_DATE { get; set; }

        [JsonProperty("sanctioN_LIMIT")]
        public decimal sanctioN_LIMIT { get; set; }

        [JsonProperty("cusT_SECTOR_DESC")]
        public string cusT_SECTOR_DESC { get; set; }

        [JsonProperty("cusT_SUB_SECTOR_DESC")]
        public string cusT_SUB_SECTOR_DESC { get; set; }

        public decimal instalment { get; set; }

        [JsonProperty("priN_REPAYMENT_FREQUENCY")]
        public string priN_REPAYMENT_FREQUENCY { get; set; }

        [JsonProperty("collateraL_DESCRIPTION")]
        public string collateraL_DESCRIPTION { get; set; }

        [JsonProperty("brancH_NAME")]
        public string brancH_NAME { get; set; }

        [JsonProperty("accounT_OFFICER")]
        public string accounT_OFFICER { get; set; }

        public string riskRating { get; set; }

        public bool confirmed { get; set; }
        public bool approved { get; set; }
        public bool isResolved { get; set; }

        public DateTime processedDate { get; set; }
        public Guid loanFileBatchId { get; set; }
    }
}