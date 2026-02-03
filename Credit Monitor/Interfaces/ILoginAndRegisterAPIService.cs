using Credit_Monitor.APIDto;
using Credit_Monitor.APIDto.Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface ILoginAndRegisterAPIService
    {
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto loginData);
        public Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerData);
        public Task<ResetPasswordResponseDto> UpdatePasswordAsync(ResetPasswordRequestDto resetData);
        public Task<bool> ConfirmGmail(string gmail);
    }
}
