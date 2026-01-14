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
            Console.WriteLine(result);
            return result;
        }
    }
}
