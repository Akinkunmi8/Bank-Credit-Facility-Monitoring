using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankLoanMonetrySolu.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccRegDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _account.RegisterUser(dto);

            if (result.IsSuccess == false)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _account.UserLogin(dto.Email, dto.Password);

            if (result.IsSuccess == false)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string Email)
        {
            var result = await _account.ForgortPassword(Email);

            if (result.IsSuccess == false)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] forgortPasswordResetdto dto)
        {
            var result = await _account.ResetForgotPassword(dto.email,dto.token,dto.newPassword);

            if (result.IsSuccess == false)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(changePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _account.ChangePassword(
                userId,
                dto.OldPassword,
                dto.NewPassword
            );

            if (result.IsSuccess == false)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
