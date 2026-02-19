using Newtonsoft.Json;

namespace Credit_Monitor.Interfaces
{
    public class DiscrepancyRemarkResponse
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        // In case the API eventually returns the actual remarks in a 'data' field
        // You can add: public string? Remarks { get; set; }
    }
}
