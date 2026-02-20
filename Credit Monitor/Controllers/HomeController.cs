using System.Diagnostics;
using System.Security.Claims;
using Credit_Monitor.APIDto.Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.Services;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Credit_Monitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userservice;
        private readonly ILogger<HomeController> _logger; 

        public HomeController(IUserService userservice, ILogger<HomeController> logger)
        {
            _userservice = userservice;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token // Try both decodedToken or token depending on how your backend expects it
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
  
            // Map ViewModel to DTO
            var resetDto = new ResetPasswordRequestDto
            {
                Email = model.Email,
                Token = model.Token,
                NewPassword = model.NewPassword
            };

            // Call the function we just implemented
            var result = await _userservice.UpdatePasswordAsync(resetDto);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfully.";
                return RedirectToAction("Login");
            }

            // Add the API's error message (e.g., "Invalid Token") to the screen
            ModelState.AddModelError(string.Empty, result.ResponseMessage);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Access Denied for IP: {RemoteIp} at {Time}",
                HttpContext.Connection.RemoteIpAddress, DateTime.Now);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt with invalid ModelState for user: {Username}", model.Username);
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                _logger.LogWarning("Login attempted with empty credentials.");
                ModelState.AddModelError(string.Empty, "Login details cannot be empty.");
                return View(model);
            }

            var authResult = await _userservice.Authenticate(model.Username, model.Password);

            if (authResult?.Response == null)
            {
                _logger.LogError("Authentication service returned null response for user: {Username}", model.Username);
                ModelState.AddModelError(string.Empty, "The authentication service is currently unavailable.");
                return View(model);
            }

            // Replacing Console.WriteLine with a proper log
            _logger.LogInformation("Auth status for {Username}: {Status}", model.Username, authResult.Response.Status);

            if (authResult.Response.Status == "Success" && authResult.User != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, authResult.User.Role ?? "User"),
                    new Claim("Region", authResult.User.Region ?? "N/A"),
                    new Claim("LastLogin", DateTime.Now.ToString("g")),
                    new Claim("JWToken", authResult.User.Token ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("User {Username} signed in via Cookies. Redirecting to {Role} dashboard.",
                    model.Username, authResult.User.Role);

                if (authResult.User.Role?.ToLower() != " " && !string.IsNullOrEmpty(authResult.User.Region))
                {
                    return RedirectToAction($"{authResult.User.Role}Dashboard", "Dashboard");
                }                            
            }

            _logger.LogWarning("Login failed for {Username}: {Message}", model.Username, authResult.Response.Message);
            ModelState.AddModelError(string.Empty, authResult.Response.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userservice.RegisterUserAsync(model);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successful registration for {Email}", model.Email);
                TempData["SuccessMessage"] = result.ResponseMessage;
                return RedirectToAction("Login");
            }

            _logger.LogWarning("Registration failed for {Email}: {Message}", model.Email, result.ResponseMessage);
            ModelState.AddModelError(string.Empty, result.ResponseMessage);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPasswordApi([FromBody] ForgotPasswordViewModel model)
        {
            _logger.LogInformation("ForgotPassword API called for {Email}", model.Email);
            var successful = await _userservice.ForgotPassword(model.Email);

            return Json(new
            {
                isSuccess = true, 
                responseMessage = "If an account exists, a reset link has been sent.",
                token = (string)null,
                user = (object)null
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = User.Identity?.Name;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            _logger.LogInformation("User {Username} logged out and session cleared.", user ?? "Unknown");
            return RedirectToAction("Index", "Home");
        }
    }
}
