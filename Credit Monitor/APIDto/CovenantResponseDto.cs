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

    public class UpdateCovenantRequestDto
    {
        // Add the ID so the API knows which record to update
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("covenantType")]
        public string CovenantType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        // Using int to match the "0" in your JSON
        [JsonProperty("frequency")]
        public int Frequency { get; set; }

        [JsonProperty("applicationId")]
        public Guid ApplicationId { get; set; }
    }

    public class UpdateCovenantResponseDto
    {
        [JsonProperty("$id")]
        public string ReferenceId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Helper property to check success based on the message content
        public bool IsSuccess => Message?.Contains("successfully", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}