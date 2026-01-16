using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;
using Microsoft.AspNetCore.Identity.Data;

namespace Credit_Monitor.APICore
{
    public class LoginAndRegisterAPIService : ILoginAndRegisterAPIService
    {
        private readonly IBackendAPIService _apiService;

        public LoginAndRegisterAPIService(IBackendAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginData)
        {
            var endpoint = "/api/account/login";
            var result = await _apiService.PostAsync<LoginResponseDto>(loginData, endpoint);
            return result;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerData)
        {
            var endpoint = "/api/account/register";
            var result = await _apiService.PostAsync<RegisterResponseDto>(registerData, endpoint);
            return result;
        }
    }
}
