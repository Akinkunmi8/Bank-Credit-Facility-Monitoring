using Newtonsoft.Json;

namespace Credit_Monitor.APIDto
{
    namespace Credit_Monitor.APIDto
    {
        public class ResetPasswordRequestDto
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("newPassword")]
            public string NewPassword { get; set; }
        }

        public class ResetPasswordResponseDto
        {
            [JsonProperty("isSuccess")]
            public bool IsSuccess { get; set; }

            [JsonProperty("responseMessage")]
            public string ResponseMessage { get; set; }

            [JsonProperty("$id")]
            public string Id { get; set; }
        }
    }
}
