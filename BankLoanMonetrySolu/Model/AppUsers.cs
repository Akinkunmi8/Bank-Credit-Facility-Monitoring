using Microsoft.AspNetCore.Identity;

namespace BankLoanMonetrySolu.Model
{
    public class AppUsers : IdentityUser
    {
        public string? Fullname { get; set; }
        public string? Region { get; set; }
        public bool? IsActive { get; set; }
    }
}
