using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.Data;
using Credit_Monitor.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Credit_Monitor.APIDto;

namespace Credit_Monitor.Services
{
    public class UserService : IUserService
    {
        private readonly ILoginAndRegisterAPIService _loginAndRegisterAPIService;

        public UserService(ILoginAndRegisterAPIService loginAndRegisterAPIService)
        {
            _loginAndRegisterAPIService = loginAndRegisterAPIService;
        }
        public async Task<LoginResponseViewModel> Authenticate(string username, string password)
        {
            var loginpayload = new LoginRequestDto
            {
                email = username,
                password = password
            };

            var apiResponse = await _loginAndRegisterAPIService.LoginAsync(loginpayload);
            if (apiResponse == null || !apiResponse.isSuccess)
            {
                string message = apiResponse?.responseMessage ?? "Invalid login attempt.";
                return GetResponse(message, "Failed", null);
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(apiResponse.token);

            var UserVM = new UserViewModel
            {
                Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "Unknown",
                Region = jwtToken.Claims.FirstOrDefault(c => c.Type == "Region")?.Value ?? "N/A",
                Role = jwtToken.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value ?? "Guest"
            };

            return GetResponse("Login successful!", "Success", UserVM);
        }

        private static LoginResponseViewModel GetResponse(string message, string status, UserViewModel? UserVM )
        {
            return new LoginResponseViewModel
            {
                Response = new ResponseViewModel
                {
                    Message = message,
                    Status = status
                },
                User = UserVM
            };
        }

    }
}
