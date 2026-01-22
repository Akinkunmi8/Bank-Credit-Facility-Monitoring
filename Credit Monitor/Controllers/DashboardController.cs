using System.Security.Claims;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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
            return View(await GetDashboardData(null, 1, region));
            //return View(staffDashboardData); 
        }

        // GET: Dashboard/FilterLoans (The AJAX call)
        public async Task<IActionResult> FilterLoans(string searchTerm, int page = 1)
        {
            var region = User.FindFirst("Region")?.Value;
            if (string.IsNullOrEmpty(region)) return Unauthorized();

            var viewModel = await GetDashboardData(searchTerm, page, region);
            return PartialView("_LoanTablePartial", viewModel);
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

        // Helper method to keep code DRY
        private async Task<StaffDashboardViewModel> GetDashboardData(string searchTerm, int page, string region)
        {
            int pageSize = 10;

            // 1. Get the raw data filtered by Region from your service
            var allLoans = await _loanService.GetPendingConfirmedLoansByRegion(region);

            // 2. Apply Search Filter if user typed something
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allLoans = allLoans.Where(l =>
                    l.accounT_NAME.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    l.id.ToString().Contains(searchTerm)
                ).ToList();
            }

            // 3. Calculate Pagination
            int totalItems = allLoans.Count();
            var pagedLoans = allLoans
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 4. Populate the full ViewModel including the API counts
            return new StaffDashboardViewModel
            {
                // Real counts from your DashboardAPI
                TotalPendingLoans = await _dashboardapi.GetTotalPending(region),
                TotalConfirmedLoans = await _dashboardapi.GetTotalConfirmed(region),
                TotalRejectedLoans = await _dashboardapi.GetTotalRejected(region),
                WithinTAT = await _dashboardapi.GetWithinTAT(region),
                BeyondTAT = await _dashboardapi.GetBeyondTAT(region),

                // Table & Pagination data
                loans = pagedLoans,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchTerm = searchTerm
            };
        }


    }
}
