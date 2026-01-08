using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Monitor.Controllers
{
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        public async Task<IActionResult> ValidateCAT(string AccNum)
        {
            if (string.IsNullOrEmpty(AccNum))
            {
                return BadRequest("Account Number is required.");
            }

            var finnacle = await _loanService.GetPendingConfirmedLoanByAccNumAsync(AccNum);
            var cat = await _loanService.GetCustomerLoanDetailsfromCATAsync(AccNum);
            var discrepancies = await _loanService.ValidateLoanDiscrepancies(AccNum);

            var viewModel = new ValidateCATPageViewModel
            {
                FinacleData = finnacle,
                CatData = cat,
                Discrepancies = discrepancies
            };

            return View(viewModel);
        }
    }
}
