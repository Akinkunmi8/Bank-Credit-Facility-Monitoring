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
        private readonly IDashboardAPI _dashboardapi;
        public DashboardController(ILoanService loanService, IDashboardAPI dashboardapi)
        {
            _loanService = loanService;
            _dashboardapi = dashboardapi;
        }

        [Authorize(Roles = "staff")]
        public async Task<IActionResult> Staff()
        {
            var region = User.FindFirst("Region")?.Value;

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            var loan = await _loanService.GetPendingConfirmedLoansByRegion(region);
            var staffDashboardData = new StaffDashboardViewModel
            {
                loans = loan,
                TotalPendingLoans = await _dashboardapi.GetTotalPending(region),
                TotalConfirmedLoans = await _dashboardapi.GetTotalConfirmed(region),
                TotalRejectedLoans = await _dashboardapi.GetTotalRejected(region),
                WithinTAT = await _dashboardapi.GetWithinTAT(region),
                BeyondTAT = await _dashboardapi.GetBeyondTAT(region)
            };
            return View(staffDashboardData); 
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
