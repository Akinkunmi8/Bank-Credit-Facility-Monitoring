using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    public class StatusUpdateRequestDto
    {
        public string ApplicationId { get; set; }
        public string Action { get; set; } // "reject" or "confirm"
    }

    public class StatusUpdateResponseDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
