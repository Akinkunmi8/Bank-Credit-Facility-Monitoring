using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
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

        public AccountService(
            DALContext context,
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IConfiguration configuration, IOptions<JwtSettings> jwtOptions,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
        }
        public async Task<AccUserResDto> RegisterUser(AccRegDto user)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
                return new AccUserResDto { IsSuccess = false, ResponseMessage = "User already exists" };

            // Create new user
            var newUser = new AppUsers
            {
                UserName = user.Email,
                Email = user.Email,
                Fullname = user.Fullname,
                Region = user.Region,
                IsActive = true
            };

            // Create user with password
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
                return new AccUserResDto
                {
                    IsSuccess = false,
                    ResponseMessage = string.Join(", ", result.Errors.Select(e => e.Description))
                };

            // Assign role submitted during registration
            if (!string.IsNullOrWhiteSpace(user.Role))
            {
                // Check if role exists, if not create it
                var roleExists = await _roleManager.RoleExistsAsync(user.Role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(user.Role));
                }

                // Assign role to user
                await _userManager.AddToRoleAsync(newUser, user.Role);
            }

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
                    //UserId = Guid.Parse(user.Id),
                    //Fullname = user.Fullname,
                    //Email = user.Email,
                    //Region = user.Region,
                    //Role = roles.FirstOrDefault(),
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

            // Frontend reset password URL
            var resetUrl =
                $"{_configuration["AppSettings:ResetPasswordUrl"]}" +
                $"?email={user.Email}&token={encodedToken}";

        //    await _emailService.SendEmailAsync(
        //        user.Email,
        //        "Reset Your Password",
        //        $@"
        //<p>Hello {user.UserName},</p>
        //<p>Click the link below to reset your password:</p>
        //<p><a href='{resetUrl}'>Reset Password</a></p>
        //<p>This link expires soon.</p>"
        //    );

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
