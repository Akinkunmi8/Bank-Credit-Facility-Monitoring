using Newtonsoft.Json;
using System.Collections.Generic;

namespace Credit_Monitor.APIDto
{
    public class CovenantResponseDto
    {
        [JsonProperty("appId")]
        public Guid AppId { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        // Change from Wrapper to direct List
        [JsonProperty("financials")]
        public List<FinancialCovenantDto> Financials { get; set; } = new();

        // Change from Wrapper to direct List
        [JsonProperty("nonFinancials")]
        public List<NonFinancialCovenantDto> NonFinancials { get; set; } = new();

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class FinancialCovenantDto
    {
        public int Id { get; set; }
        public string CovenantType { get; set; }
        public string Description { get; set; }
        public decimal TargetValue { get; set; }
        public int Frequency { get; set; }
        public string DataSource { get; set; }
    }

    public class NonFinancialCovenantDto
    {
        public int Id { get; set; }
        public string CovenantType { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
    }
}