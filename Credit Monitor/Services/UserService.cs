using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.Data;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Services
{
    public class UserService : IUserService
    {
        private readonly ILoginAndRegisterAPIService _loginAndRegisterAPIService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger; 

        public UserService(ILoginAndRegisterAPIService loginAndRegisterAPIService, IMapper mapper, ILogger<UserService> logger)
        {
            _loginAndRegisterAPIService = loginAndRegisterAPIService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<LoginResponseViewModel> Authenticate(string username, string password)
        {
            _logger.LogInformation("Authentication attempt for user: {Username}", username);

            try
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

                    _logger.LogWarning("Login failed for {Username}. Reason: {Reason}", username, message);

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

                _logger.LogInformation("User {Username} successfully authenticated. Role: {Role}, Region: {Region}",
                    UserVM.Username, UserVM.Role, UserVM.Region);

                return GetResponse("Login successful!", "Success", UserVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error during authentication process for {Username}", username);
                throw;
            }
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

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterViewModel model)
        {
            _logger.LogInformation("Registering new user: {Email}", model.Email);

            try
            {
                var registerRequest = _mapper.Map<RegisterRequestDto>(model);
                var registerResponse = await _loginAndRegisterAPIService.RegisterAsync(registerRequest);

                _logger.LogInformation("Registration request completed for {Email}. Success: {Status}",
                    model.Email, registerResponse.IsSuccess);

                return registerResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for {Email}", model.Email);
                throw;
            }
        }

        public async Task<bool> ForgotPassword(string gmail)
        {
            _logger.LogInformation("Password reset requested for: {Email}", gmail);

            var success = await _loginAndRegisterAPIService.ConfirmGmail(gmail);

            if (!success)
            {
                _logger.LogWarning("Password reset requested for non-existent or invalid email: {Email}", gmail);
            }

            return success;
        }
    }

        
}
