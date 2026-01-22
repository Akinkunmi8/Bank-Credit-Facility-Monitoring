namespace Credit_Monitor.APIsDto
{
    public class LoginResponseDto
    {
        // $id metadata is handled by the serializer
        public bool isSuccess { get; set; }
        public string responseMessage { get; set; }
        public string token { get; set; }
        public UserDetailsDto user { get; set; }
    }

    public class UserDetailsDto
    {
        public Guid userId { get; set; }

        // These are nullable in your JSON example
        public string fullname { get; set; }
        public string region { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public bool isActive { get; set; }
    }
}
