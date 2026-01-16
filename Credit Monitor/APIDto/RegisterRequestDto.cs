using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    public class RegisterRequestDto
    {
        [JsonProperty("fullname")]
        public string FullName { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;
    }
}
