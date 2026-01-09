namespace BankLoanMonetrySolu.DTO
{
    public class AccUserResDto
    {
        public bool? IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
        public string? Token { get; set; }
        public AccUserDto? User { get; set; }

    }

    public class AccRegDto
    {
        public string? Fullname { get; set; }
        public string? Region { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AccUserDto
    {
        public Guid UserId { get; set; }
        public string? Fullname { get; set; }
        public string? Region { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }

    public class forgortPasswordResetdto
    {
        public string? newPassword { get; set; }
        public string? email { get; set; }
        public string? token { get; set; }
    }

    public class changePasswordDto
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
    public class  loginDto
    {
        public string Email {  get; set; }
        public string Password { get; set; }
    }

}
