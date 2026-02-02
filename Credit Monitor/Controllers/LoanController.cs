using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Monitor.Controllers
{
    [Authorize]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanController> _logger; 
  
        public LoanController(ILoanService loanService, ILogger<LoanController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        public async Task<IActionResult> ValidateCAT(string AccNum)
        {
           
            _logger.LogInformation("ValidateCAT initiated for Account Number: {AccNum}", AccNum);

            if (string.IsNullOrEmpty(AccNum))
            {
                _logger.LogWarning("ValidateCAT failed: Account Number was null or empty.");
                return BadRequest("Account Number is required.");
            }

            try
            {
                var finnacle = await _loanService.GetPendingConfirmedLoanByAccNumAsync(AccNum);
                var cat = await _loanService.GetCustomerLoanDetailsfromCATAsync(AccNum);
                var discrepancies = await _loanService.ValidateLoanDiscrepancies(AccNum);

                var viewModel = new ValidateCATPageViewModel
                {
                    FinacleData = finnacle,
                    CatData = cat,
                    Discrepancies = discrepancies
                };

                _logger.LogInformation("Successfully retrieved CAT validation data for AccNum: {AccNum}", AccNum);

                return View(viewModel);
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "An error occurred while validating CAT for Account Number: {AccNum}", AccNum);
                return View("Error"); 
            }
        }
    }
}