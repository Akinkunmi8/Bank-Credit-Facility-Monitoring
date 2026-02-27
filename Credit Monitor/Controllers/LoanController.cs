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

        public async Task<IActionResult> ResolutionCenter(string id) // id = AccNum
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            // 1. Get real-time data
            var finnacle = await _loanService.GetPendingConfirmedLoanByAccNumAsync(id);
            var cat = await _loanService.GetCustomerLoanDetailsfromCATAsync(id);
            var discrepancies = await _loanService.ValidateLoanDiscrepancies(id);

            // 2. Get and Map existing remarks using the AppId (Finnacle ID)
            var savedRemarks = await _loanService.GetSavedResolutionsMapped(finnacle!.Id);

            var viewModel = new ValidateCATPageViewModel
            {
                FinacleData = finnacle,
                CatData = cat,
                Discrepancies = discrepancies
            };

            // 3. The Stitching: 
            // We pass the mapped list to the ViewBag. 
            // The View will use this to pre-fill the form cards.
            ViewBag.ExistingResolutions = savedRemarks ?? new List<ResolutionItem>();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResolutions(Guid AppId, string AccountNum, List<ResolutionItem> Resolutions)
        {
            // 1. Ask the service to do the heavy lifting
            var (successCount, failureCount) = await _loanService.ProcessBulkResolutionsAsync(AppId, Resolutions);

            // 2. Provide feedback based on the results
            if (successCount > 0 && failureCount == 0)
            {
                TempData["Success"] = $"Successfully submitted all {successCount} resolutions.";
            }
            else if (successCount > 0 && failureCount > 0)
            {
                TempData["Warning"] = $"Partial Success: {successCount} saved, but {failureCount} failed. Please review.";
            }
            else
            {
                TempData["Error"] = "Failed to submit resolutions. Please check your connection and try again.";
            }

            // 3. Return to the Resolution Center to see updated state
            return RedirectToAction("ResolutionCenter", new { id = AccountNum });
        }
    }
}