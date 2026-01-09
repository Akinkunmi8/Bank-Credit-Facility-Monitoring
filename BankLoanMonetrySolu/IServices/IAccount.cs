using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface IAccount
    {
        Task<AccUserResDto> RegisterUser (AccRegDto user);
        Task<AccUserResDto> UserLogin (string email, string password);
        Task<AccUserResDto> ForgortPassword (string email);
        Task<AccUserResDto> ResetForgotPassword(string email, string token, string newPassword);
        Task<AccUserResDto> ChangePassword(string userId,string oldPassword, string newPassword);
       
    }
}
