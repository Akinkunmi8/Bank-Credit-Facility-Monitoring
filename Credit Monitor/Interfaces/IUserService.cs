using Credit_Monitor.APIDto;
using Credit_Monitor.APIDto.Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponseViewModel> Authenticate(string username, string password);
        Task<RegisterResponseDto> RegisterUserAsync(RegisterViewModel model);
        Task<ResetPasswordResponseDto> UpdatePasswordAsync(ResetPasswordRequestDto resetData);
        Task<bool> ForgotPassword(string gmail);
    }

}
