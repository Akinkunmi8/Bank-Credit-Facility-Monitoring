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
        private IEmailServices _emailServices;

        public AccountService(
            DALContext context,
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IConfiguration configuration, IOptions<JwtSettings> jwtOptions,
            RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor,
            IEmailServices emailServices)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _emailServices = emailServices;
        }
        public async Task<AccUserResDto> RegisterUser(AccRegDto user)
        {
            // 1️⃣ Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "User already exists" };

            // 2️⃣ Create new user
            var newUser = new AppUsers
            {
                UserName = user.Email,
                Email = user.Email,
                Fullname = user.Fullname,
                Region = user.Region,
                IsActive = true
            };

            // 3️⃣ Create user with password
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = string.Join(", ", result.Errors.Select(e => e.Description))
                };

            // 4️⃣ Assign role submitted during registration
            if (!string.IsNullOrWhiteSpace(user.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(user.Role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(user.Role));
                }

                await _userManager.AddToRoleAsync(newUser, user.Role);
            }

            // 5️⃣ Populate Audit table for user registration
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            var audit = new Audit
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id,           // Newly created user ID
                Action = AuditAction.CreateUser,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            _context.audits.Add(audit);
            await _context.SaveChangesAsync();

            // 6️⃣ Return response
            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "User registered successfully"
            };
        }



        // 2️⃣ Login User
        public async Task<AccUserResDto> UserLogin(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "User not found" };

            if (user.IsActive != true)
            {
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Account is inactive"
                };
            }


            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "Invalid credentials" };

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user);

            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "Login successful",
                Token = token.Result,
                User = new AccUserDto
                {
                    IsActive = user.IsActive
                }
            };
        }

        public async Task<AccUserResDto> ForgortPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AccUserResDto
                {
                    IsSuccess = true,
                    ResponseMessage = "If the email exists, a password reset link has been sent."
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var resetUrl =
                $"{_configuration["AppSettings:ResetPasswordUrl"]}" +
                $"?email={user.Email}&token={encodedToken}";

            try
            {
                await _emailServices.SendEmailAsync(
                    user.Email,
                    "Account Recovery",
                    resetUrl
                );
            }
            catch (Exception ex)
            {
                // log this properly (Serilog / ILogger)
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Failed to send email: {ex.Message}"
                };
            }

            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "Password reset link has been sent to your email"
            };
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

        public async Task<AccUserResDto> ChangePassword(string userId,string oldPassword,string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = "User not found"
                };
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                oldPassword,
                newPassword
            );

            if (!result.Succeeded)
            {
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = string.Join(", ",
                        result.Errors.Select(e => e.Description))
                };
            }

            return new AccUserResDto
            {
                IsSuccess = true,
                ResponseMessage = "Password changed successfully"
            };
        }


        // JWT token generator
        private async Task <string> GenerateJwtToken(AppUsers user)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
                throw new Exception("JWT Key is missing in configuration");
            var roles = await _userManager.GetRolesAsync(user);

            // Take first role if you want only one
            var role = roles.FirstOrDefault() ?? "User";

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("Fullname", user.Fullname ?? ""),
                new Claim("Region", user.Region ?? ""),
                new Claim(ClaimTypes.Role, role ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
