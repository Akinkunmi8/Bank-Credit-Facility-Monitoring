namespace Credit_Monitor.Models
{
    public enum UserRole
    {
        Staff,
        Supervisor
    }
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
        public string? Region { get; set; }
    }
}
