using Newtonsoft.Json;
namespace Credit_Monitor.APIDto
{
    public class RegisterResponseDto
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        [JsonProperty("token")]
        public string? Token { get; set; }

        [JsonProperty("user")]
        public object? User { get; set; } // You can replace 'object' with a specific UserDto if needed
    }
}
