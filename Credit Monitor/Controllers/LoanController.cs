using Credit_Monitor.APIDto;
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

        [Authorize(Roles = "CMO")]
        [HttpGet]
        public async Task<IActionResult> ReviewResolution(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Error"] = "Invalid account number.";
                return RedirectToAction(
                    "CMODashboard",
                    "Dashboard",
                    new { status = "CMOReview" });
            }

            try
            {
                // 1. Get latest Finacle data
                var finacle =
                    await _loanService
                        .GetPendingConfirmedLoanByAccNumAsync(id);

                if (finacle == null)
                {
                    TempData["Error"] =
                        "Loan application could not be found.";

                    return RedirectToAction(
                        "CMODashboard",
                        "Dashboard",
                        new { status = "CMOReview" });
                }

                // 2. Get latest CAT data
                var cat =
                    await _loanService
                        .GetCustomerLoanDetailsfromCATAsync(id);

                // 3. Re-run LIVE comparison
                var discrepancies =
                    await _loanService
                        .ValidateLoanDiscrepancies(id);

                // 4. Get CMO instruction + RM/AO response
                var savedResolutions =
                    await _loanService
                        .GetSavedResolutionsMapped(finacle.Id);

                var viewModel = new ValidateCATPageViewModel
                {
                    FinacleData = finacle,
                    CatData = cat,
                    Discrepancies = discrepancies
                };

                ViewBag.ExistingResolutions =
                    savedResolutions ?? new List<ResolutionItem>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading CMO resolution review for Account {AccountNumber}",
                    id);

                TempData["Error"] =
                    "Unable to load the resolution review.";

                return RedirectToAction(
                    "CMODashboard",
                    "Dashboard",
                    new { status = "CMOReview" });
            }
        }

        [Authorize(Roles = "AO,RM")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitResolutionResponses(Guid AppId,string AccountNum,List<ResolutionItem> Resolutions)
        {
            if (AppId == Guid.Empty)
            {
                TempData["Error"] = "Invalid loan application.";

                return RedirectToAction(
                    "ResolutionCenter",
                    new { id = AccountNum });
            }

            if (Resolutions == null || Resolutions.Count == 0)
            {
                TempData["Error"] =
                    "No discrepancy responses were submitted.";

                return RedirectToAction(
                    "ResolutionCenter",
                    new { id = AccountNum });
            }

            try
            {
                // 1. Save AO/RM response against every discrepancy
                var (successCount, failureCount) =
                    await _loanService.ProcessBulkResolutionResponsesAsync(
                        AppId,
                        Resolutions);

                if (failureCount > 0)
                {
                    TempData["Error"] =
                        $"{failureCount} response(s) could not be saved. " +
                        "The application was not submitted to CMO.";

                    return RedirectToAction(
                        "ResolutionCenter",
                        new { id = AccountNum });
                }

                if (successCount == 0)
                {
                    TempData["Error"] =
                        "No resolution responses were saved.";

                    return RedirectToAction(
                        "ResolutionCenter",
                        new { id = AccountNum });
                }

                // 2. All individual responses saved.
                // Submit the entire application back to CMO.
                var submitResponse =
                    await _loanService.SubmitResolutionForReviewAsync(AppId);

                if (!submitResponse.IsSuccess)
                {
                    TempData["Error"] =
                        submitResponse.ResponseMessage
                        ?? "Unable to submit the application to CMO.";

                    return RedirectToAction(
                        "ResolutionCenter",
                        new { id = AccountNum });
                }

                TempData["Success"] =
                    "All corrective responses were submitted successfully. " +
                    "The application has been returned to CMO for re-review.";

                // 3. Remove AO from the resolution screen.
                return RedirectToAction(
                    "AODashboard",
                    "Dashboard",
                    new { status = "Unresolved" });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error submitting AO/RM resolution responses for AppId {AppId}",
                    AppId);

                TempData["Error"] =
                    "An error occurred while submitting the resolution responses.";

                return RedirectToAction(
                    "ResolutionCenter",
                    new { id = AccountNum });
            }
        }

        [Authorize(Roles = "CMO")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitResolutionDecision(ResolutionDecisionViewModel model)
        {
            if (model.ApplicationId == Guid.Empty)
            {
                TempData["Error"] =
                    "Invalid loan application.";

                return RedirectToAction(
                    "CMODashboard",
                    "Dashboard",
                    new { status = "CMOReview" });
            }

            try
            {
                string actionType;

                if (string.Equals(
                    model.Decision,
                    "Accept",
                    StringComparison.OrdinalIgnoreCase))
                {
                    actionType = "AcceptResolution";
                }
                else if (string.Equals(
                    model.Decision,
                    "Return",
                    StringComparison.OrdinalIgnoreCase))
                {
                    if (!model.Deadline.HasValue)
                    {
                        TempData["Error"] =
                            "A resolution deadline is required when returning the application.";

                        return RedirectToAction(
                            "ReviewResolution",
                            new { id = model.AccountNumber });
                    }

                    actionType = "ReturnResolution";
                }
                else
                {
                    TempData["Error"] =
                        "Invalid resolution decision.";

                    return RedirectToAction(
                        "ReviewResolution",
                        new { id = model.AccountNumber });
                }

                var response = await _loanService.ProcessStatusUpdate(
                                     model.ApplicationId,
                                     actionType,
                                     model.Deadline
                                 );

                if (!response.isSuccess)
                {
                    TempData["Error"] =
                        "Unable to update the resolution status.";

                    return RedirectToAction(
                        "ReviewResolution",
                        new { id = model.AccountNumber });
                }

                TempData["Success"] =
                    actionType == "AcceptResolution"
                        ? "Resolution accepted successfully."
                        : "Application returned to RM/AO for further resolution.";

                return RedirectToAction(
                    "CMODashboard",
                    "Dashboard",
                    new { status = "CMOReview" });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing CMO resolution decision for {ApplicationId}",
                    model.ApplicationId);

                TempData["Error"] =
                    "An error occurred while processing the resolution decision.";

                return RedirectToAction(
                    "ReviewResolution",
                    new { id = model.AccountNumber });
            }
        }
    }


}