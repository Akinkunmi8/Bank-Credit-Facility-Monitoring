using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface ILoginAndRegisterAPIService
    {
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto loginData);
    }
}
