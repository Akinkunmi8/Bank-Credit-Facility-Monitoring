using System.Diagnostics;
using System.Security.Claims;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Monitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userservice;

        public HomeController(IUserService userservice)
        {
            _userservice = userservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Login details cannot be empty.");
                return View(model);
            }

            var authResult = await _userservice.Authenticate(model.Username, model.Password);
            if (authResult?.Response == null)
            {
                ModelState.AddModelError(string.Empty, "The authentication service is currently unavailable.");
                return View(model);
            }


            if (authResult.Response.Status == "Success" && authResult.User != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, authResult.User.Role ?? "User"),
                    new Claim("Region", authResult.User.Region ?? "N/A"),
                    new Claim("LastLogin", DateTime.Now.ToString("g"))
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
                if(authResult.User.Role == "Staff" && !string.IsNullOrEmpty(authResult.User.Region))
                {
                    return RedirectToAction("Staff", "Dashboard");
                }
                if (authResult.User.Role == "Supervisor" && !string.IsNullOrEmpty(authResult.User.Region))
                {
                    return RedirectToAction("Supervisor", "Dashboard");
                }
            }

            ModelState.AddModelError(string.Empty, errorMessage: authResult.Response.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
