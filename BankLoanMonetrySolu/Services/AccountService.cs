using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using BankLoanMonetrySolu.Models;
using Contracts.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
// Added for NLog integration
using Microsoft.Extensions.Logging;

namespace BankLoanMonetrySolu.Services
{
    public class AccountService : IAccount
    {
        private readonly DALContext _context;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<AccountService> _logger; // 1. Inject Logger

        public AccountService(
            DALContext context,
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IConfiguration configuration,
            IOptions<JwtSettings> jwtOptions,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailServices emailServices,
            ILogger<AccountService> logger) // 2. Add to Constructor
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _emailServices = emailServices;
            _logger = logger;
        }

        public async Task<AccUserResDto> RegisterUser(AccRegDto user)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", user.Email);

                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: User {Email} already exists", user.Email);
                    return new AccUserResDto { IsSuccess = false, ResponseMessage = "User already exists" };
                }

                var newUser = new AppUsers
                {
                    UserName = user.Email,
                    Email = user.Email,
                    Fullname = user.Fullname,
                    Region = user.Region,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Identity error during registration for {Email}: {Errors}", user.Email, errors);
                    return new AccUserResDto { IsSuccess = false, ResponseMessage = errors };
                }

                if (!string.IsNullOrWhiteSpace(user.Role))
                {
                    var roleExists = await _roleManager.RoleExistsAsync(user.Role);
                    if (!roleExists)
                    {
                        _logger.LogInformation("Creating new role: {Role}", user.Role);
                        await _roleManager.CreateAsync(new IdentityRole(user.Role));
                    }
                    await _userManager.AddToRoleAsync(newUser, user.Role);
                }

                _logger.LogInformation("User {Email} registered successfully with ID {Id}", user.Email, newUser.Id);

                // Audit Logging
                var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
                var audit = new Audit
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Id,
                    Action = AuditAction.CreateUser,
                    IpAddress = ipAddress,
                    CreatedAt = DateTime.UtcNow
                };

                _context.audits.Add(audit);
                await _context.SaveChangesAsync();

                return new AccUserResDto { IsSuccess = true, ResponseMessage = "User registered successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error during registration for {Email}", user.Email);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "An internal error occurred." };
            }
        }

        public async Task<AccUserResDto> UserLogin(string email, string password)
        {
            _logger.LogInformation("Login attempt for user: {Email}", email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User {Email} not found", email);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "User not found" };
            }

            if (user.IsActive != true)
            {
                _logger.LogWarning("Login blocked: Account {Email} is inactive", email);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Account is inactive" };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed: Invalid credentials for {Email}", email);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Invalid credentials" };
            }

            _logger.LogInformation("User {Email} logged in successfully", email);
            var token = await GenerateJwtToken(user);

            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "Login successful",
                Token = token,
                User = new AccUserDto { IsActive = user.IsActive }
            };
        }

        public async Task<AccUserResDto> ForgortPassword(string email)
        {
            _logger.LogInformation("Password reset requested for {Email}", email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Password reset: Email {Email} not found in system", email);
                return new AccUserResDto { IsSuccess = true, ResponseMessage = "If the email exists, a link has been sent." };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetUrl = $"{_configuration["AppSettings:ResetPasswordUrl"]}?email={user.Email}&token={encodedToken}";

            try
            {
                await _emailServices.SendEmailAsync(user.Email, "Account Recovery", resetUrl);
                _logger.LogInformation("Reset email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Email service failure." };
            }

            return new AccUserResDto { IsSuccess = true, ResponseMessage = "Link sent successfully" };
        }

        public async Task<AccUserResDto> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            _logger.LogInformation("ChangePassword attempt for UserId: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new AccUserResDto { IsSuccess = false, ResponseMessage = "User not found" };

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Password change failed for {UserId}", userId);
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Update failed." };
            }

            _logger.LogInformation("Password changed successfully for {UserId}", userId);
            return new AccUserResDto { IsSuccess = true, ResponseMessage = "Success" };
        }

        private async Task<string> GenerateJwtToken(AppUsers user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim("Fullname", user.Fullname ?? ""),
                    new Claim(ClaimTypes.Role, role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "JWT Generation failed for user {Email}", user.Email);
                throw;
            }
        }

        public async Task<AccUserResDto> ResetForgotPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Invalid request" };

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = string.Join(", ", result.Errors.Select(e => e.Description))
                };

            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "Password reset successful"
            };
        }
    }
}