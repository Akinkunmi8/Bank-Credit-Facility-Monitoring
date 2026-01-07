using System.Security.Claims;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Credit_Monitor.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILoanService _loanService;
        public DashboardController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Staff()
        {
            var region = User.FindFirst("Region")?.Value;

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            var loans = await _loanService.GetPendingConfirmedLoansByRegion(region);
            return View(loans); 
        }

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Supervisor()
        {
            var region = User.FindFirst("Region")?.Value;

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            var loans = await _loanService.GetPendingConfirmedLoansByRegion(region);
            return View(loans);
        }

        public async Task<IActionResult> CustomerDetails(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid loan ID.");
            }
            var loan = await _loanService.GetPendingConfirmedLoanByIdAsync(id);
            return View(loan);
        }


    }
}
