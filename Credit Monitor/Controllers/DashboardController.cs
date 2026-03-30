using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Services;
using Credit_Monitor.ViewModel;
using Credit_Monitor.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Monitor.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IDashboardAPI _dashboardapi;
        private readonly IFileService _fileService;
        private readonly ICovenantTrackerService _covenantService;
        private string _storagePath;
        private readonly IMapper _mapper;

        public DashboardController(
            ILoanService loanService,
            IDashboardAPI dashboardapi,
            IFileService fileService,
            ICovenantTrackerService covenantService,
            IConfiguration configuration,
            IMapper mapper) 
        {
            _loanService = loanService;
            _dashboardapi = dashboardapi;
            _fileService = fileService;
            _covenantService = covenantService;
            _storagePath = configuration["FileSettings:StoragePath"];
            _mapper = mapper;
        }

        [Authorize(Roles = "CMO")]
        public async Task<IActionResult> CMODashboard(string status)
        {
            var region = User.FindFirst("Region")?.Value;
            if (status == null) status = "Unreviewed";

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            ViewData["Title"] = status switch
            {
                "Unreviewed" => "Unreviewed Loan Applications",
                "Reviewed" => "Reviewed & Confirmed Loans",
                "Unresolved" => "Unresolved (Rejected) Loans",
                "Resolved" => "Resolved Applications",
                "WithinTATUnreviewed" => "Unreviewed Loans (Within TAT)",
                "WithinTATUnresolved" => "Unresolved Loans (Within TAT)",
                "BeyondTATUnreviewed" => "Unreviewed Loans (Beyond TAT)",
                "BeyondTATUnresolved" => "Unresolved Loans (Beyond TAT)",
                _ => " Unreviewed Loan Applications"
            };
            var loan = await _loanService.GetPendingConfirmedLoansByRegion(region);
            return View(await GetCMODashboardData(null, 1, region, status));
        }

        [Authorize(Roles = "AO")]
        public async Task<IActionResult> AODashboard(string status)
        {
            var region = User.FindFirst("Region")?.Value;
            if (status == null) status = "Unresolved";

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            ViewData["Title"] = status switch
            {
                "Unreviewed" => "Unreviewed Loan Applications",
                "Reviewed" => "Reviewed & Confirmed Loans",
                "Unresolved" => "Unresolved (Rejected) Loans",
                "Resolved" => "Resolved Applications",
                "WithinTATUnreviewed" => "Unreviewed Loans (Within TAT)",
                "WithinTATUnresolved" => "Unresolved Loans (Within TAT)",
                "BeyondTATUnreviewed" => "Unreviewed Loans (Beyond TAT)",
                "BeyondTATUnresolved" => "Unresolved Loans (Beyond TAT)",
                _ => "Unresolved Loan Applications"
            };
            var loan = await _loanService.GetPendingConfirmedLoansByRegion(region);
            return View(await GetAODashboardData(null, 1, region, status));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentStatus(Guid appId)
        {
            // Ensure we are using the correct Service Method
            var documents = await _fileService.GetCustomerDocumentsAsync(appId);

            // Return the partial view we refactored earlier
            return PartialView("_DocumentStatusPartial", documents);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVerification(Guid fileId, bool status)
        {
            var success = await _fileService.ToggleVerificationAsync(fileId, status);
            if (success) return Ok();
            return BadRequest("Could not update status.");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddFileProperties(Guid appId, string fileName, string docType, string contentType)
        {
            if (appId == Guid.Empty || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("Missing required file information.");
            }

            var model = new FileItemDto
            {
                AppId = appId,
                FileName = fileName,
                DocType = docType,
                ContentType = contentType,
                isVeriFied = false
            };

            try
            {
                // 1. Save the new file
                var newId = await _fileService.UploadDocumentAsync(model);

                if (newId != null)
                {
                    // 2. CRITICAL: Fetch the updated list immediately
                    var updatedList = await _fileService.GetCustomerDocumentsAsync(appId);

                    // 3. Return the HTML of the table, not just a JSON success message
                    return PartialView("_DocumentStatusPartial", updatedList);
                }

                return BadRequest("The API service could not save the record.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFileProperties(FileItemDto model)
        {
            // Without [FromBody], ASP.NET binds the form data to the model properties automatically
            if (model == null || model.Id == Guid.Empty)
            {
                return BadRequest("Invalid file data received.");
            }

            try
            {
                // 1. Update the record via your Service
                // This should hit your EditFile(appId, fileDto) API method
                var refreshedList = await _fileService.UpdateDocumentAsync(model.AppId, model);

                // 2. Return the Partial View to refresh the UI table
                return PartialView("_DocumentStatusPartial", refreshedList);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Update failed on the server.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile(Guid appId, Guid fileId)
        {
            if (appId == Guid.Empty || fileId == Guid.Empty)
            {
                return BadRequest("Invalid identifiers for deletion.");
            }

            try
            {

                var success = await _fileService.DeleteDocumentAsync(appId, fileId);

                if (success)
                {
                    // Fetch the updated list to refresh the table UI
                    var updatedList = await _fileService.GetCustomerDocumentsAsync(appId);
                    return PartialView("_DocumentStatusPartial", updatedList);
                }

                return BadRequest("The file could not be deleted from the system.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error during deletion.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCovenantStatus(Guid appId)
        {
            if (appId == Guid.Empty)
            {
                return PartialView("_CovenantStatusPartial", new CovenantViewModel());
            }

            var covenants = await _covenantService.GetCovenantDetailsAsync(appId);

            // If no records found, return an empty ViewModel instead of null
            return PartialView("_CovenantStatusPartial", covenants ?? new CovenantViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCovenant(NonFinancialCovenantVM model, Guid ApplicationId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CustomerDetails", "Dashboard", new { id = ApplicationId });
            }

            UpdateCovenantResponseDto result;

            if (model.Id == 0)
            {
                // Call the ADD service
                result = await _covenantService.CreateCovenant(model, ApplicationId);
            }
            else
            {
                // Call the UPDATE service
                result = await _covenantService.UpdateCovenant(model, ApplicationId);
            }

            if (result != null && result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result?.Message ?? "Operation failed.";
            }

            return RedirectToAction("CustomerDetails", "Dashboard", new { id = ApplicationId });
        }

        [HttpGet]
        public IActionResult ViewDocument(Guid batchId, string category, string ext)
        {
            // 1. Sanitize the extension (remove leading dots if any)
            string cleanExt = ext.Replace(".", "").ToLower();

            // 2. Build the full path securely
            // Structure: C:\CreditMonitorFiles\MainFolder\ID_Cards\da3c8...pdf
            string filePath = Path.Combine(_storagePath, category, $"{batchId}.{cleanExt}");

            // 3. Verify file existence on disk
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"The requested document is not available in storage.\nPath: {filePath}");
            }

            string contentType = GetMimeType(cleanExt);

            // Use FileStream with 'FileOptions.Asynchronous' for better performance
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            return File(fileStream, contentType);
        }

        // GET: Dashboard/FilterLoans (The AJAX call)
        public async Task<IActionResult> FilterLoans(string searchTerm, string role, string status,  int page = 1 )
        {
            var region = User.FindFirst("Region")?.Value;
            if (string.IsNullOrEmpty(region)) return Unauthorized();

            if(role == "CMS")
            {
                var supervisorVM = await GetCMSDashboardData(searchTerm, page, region, status);
                return PartialView("_ConfirmedLoanTablePartial", supervisorVM);
            }
            if (role == "AO")
            {
                var AOVM = await GetAODashboardData(searchTerm, page, region, status);
                return PartialView("_UnresolvedLoanTablePartial", AOVM);
            }
            ViewData["Title"] = status switch
            {
                "Unreviewed" => "Unreviewed Loan Applications",
                "Reviewed" => "Reviewed & Confirmed Loans",
                "Unresolved" => "Unresolved (Rejected) Loans",
                "Resolved" => "Resolved Applications",
                "Approved" => "Approved Loan Applications",
                "WithinTATUnreviewed" => "Unreviewed Loans (Within TAT)",
                "WithinTATUnresolved" => "Unresolved Loans (Within TAT)",
                "BeyondTATUnreviewed" => "Unreviewed Loans (Beyond TAT)",
                "BeyondTATUnresolved" => "Unresolved Loans (Beyond TAT)",
                "BeyondTATReviewed" => "Reviewed Loans (Beyond TAT)",
                _ => "Loan Applications"
            };
            var CMOVM = await GetCMODashboardData(searchTerm, page, region, status);
            return PartialView("_LoanTablePartial", CMOVM);
        }


        [Authorize(Roles = "CMS")]
        public async Task<IActionResult> CMSDashboard(string status)
        {
            var region = User.FindFirst("Region")?.Value;
            if (status == null) status = "Reviewed";

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            ViewData["Title"] = status switch
            {
                "Unreviewed" => "Unreviewed Loan Applications",
                "Reviewed" => "Reviewed & Confirmed Loans",
                "Unresolved" => "Unresolved (Rejected) Loans",
                "Resolved" => "Resolved Applications",
                "Approved" => "Approved Loan Applications",
                "WithinTATUnreviewed" => "Unreviewed Loans (Within TAT)",
                "WithinTATUnresolved" => "Unresolved Loans (Within TAT)",
                "BeyondTATUnreviewed" => "Unreviewed Loans (Beyond TAT)",
                "BeyondTATUnresolved" => "Unresolved Loans (Beyond TAT)",
                "BeyondTATReviewed" => "Reviewed Loans (Beyond TAT)",
                _ => " Unreviewed Loan Applications"
            };
            var loan = await _loanService.GetConfirmedLoanByRegion(region); 
            return View(await GetCMSDashboardData(null, 1, region, status));
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid applicationId, string actionType, string returnUrl, string returnDashboard, DateTime? deadline)
        {
            if (applicationId == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Invalid Application ID provided.";
                return Redirect(returnUrl);
            }

            var (isSuccess, message) = await _loanService.ProcessStatusUpdate(applicationId, actionType, deadline);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = message; 
            }
            else
            {
                TempData["ErrorMessage"] = "Failed: " + message;
            }

            return RedirectToAction(returnDashboard);
        }

        [HttpGet]
        public async Task<IActionResult> ExportLoans(string format, string searchTerm, string role)
        {
            // 1. Validation & Setup
            var region = User.FindFirst("Region")?.Value;
            if (string.IsNullOrEmpty(region)) return Unauthorized();

            // 2. Fetch Data based on Role
            IEnumerable<PendingConfirmedLoanViewModel> data = (role == "supervisor")
                ? await _loanService.GetConfirmedLoanByRegion(region)
                : await _loanService.GetPendingConfirmedLoansByRegion(region);

            // 3. Apply Search Filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                data = data.Where(l =>
                    l.accounT_NAME.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    l.id.ToString().Contains(searchTerm)
                );
            }

            var list = data.ToList();

            // 4. Handle CSV Export
            if (format.ToLower() == "csv")
            {
                var csv = new System.Text.StringBuilder();

                // 1. Full Header (Matches your ViewModel properties)
                csv.AppendLine("Application ID,Account Name,Account Number,Contract ID,Customer ID,Region,Branch," +
                               "Facility Type,Currency,Sanction Limit,Balance LCY,Balance FCY,Interest Rate," +
                               "Date Granted,Expiry Date,Days Past Due,Performance Status," +
                               "Sector,Sub-Sector,Repayment Freq (Prin),Repayment Freq (Int),Account Officer");

                foreach (var item in list)
                {
                    string currentStatus = !string.IsNullOrEmpty(item.performancE_CLASS_DESC)
                                           ? item.performancE_CLASS_DESC
                                           : "Pending Staff Review";

                    // 2. Full Data Row 
                    // We use .Replace(",", " ") on strings to ensure embedded commas don't shift CSV columns
                    csv.AppendLine($"{item.id}," +
                                   $"\"{item.accounT_NAME?.Trim().Replace(",", " ")}\"," +
                                   $"{item.accounT_NUMBER}," +
                                   $"{item.contracT_ID}," +
                                   $"{item.customeR_ID}," +
                                   $"{item.region}," +
                                   $"\"{item.brancH_NAME?.Replace(",", " ")}\"," +
                                   $"{item.facilitY_TYPE}," +
                                   $"{item.accounT_CURRENCY}," +
                                   $"{item.sanctioN_LIMIT}," +
                                   $"{item.loaN_BALANCE_LCY}," +
                                   $"{item.loaN_BALANCE_FCY}," +
                                   $"{item.inT_RATE}%," +
                                   $"{item.datE_GRANTED:yyyy-MM-dd}," +
                                   $"{item.expirY_DATE:yyyy-MM-dd}," +
                                   $"{item.dayS_PAST_DUE ?? 0}," +
                                   $"{currentStatus}," +
                                   $"\"{item.cusT_SECTOR_DESC?.Replace(",", " ")}\"," +
                                   $"\"{item.cusT_SUB_SECTOR_DESC?.Replace(",", " ")}\"," +
                                   $"{item.priN_REPAYMENT_FREQUENCY}," +
                                   $"{item.inT_REPAYMENT_FREQUENCY}," +
                                   $"\"{item.accounT_OFFICER?.Replace(",", " ")}\"");
                }

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
                return File(buffer, "text/csv", $"Full_Loan_Export_{DateTime.Now:yyyyMMdd_HHmm}.csv");
            }
            // 5. Handle PDF Export (Placeholders)
            if (format.ToLower() == "pdf")
            {
                // Add your PDF logic here later
                return BadRequest("PDF Export engine not yet configured.");
            }

            return BadRequest("Invalid format.");
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscrepancyStatus(string accNum)
        {
            if (string.IsNullOrEmpty(accNum)) return PartialView("_DiscrepancyStatusPartial", null);

            // Reuse your existing logic to get the data
            var finnacle = await _loanService.GetPendingConfirmedLoanByAccNumAsync(accNum);
            var cat = await _loanService.GetCustomerLoanDetailsfromCATAsync(accNum);
            var discrepancies = await _loanService.ValidateLoanDiscrepancies(accNum);
            var savedRemarks = await _loanService.GetSavedResolutionsMapped(finnacle!.Id);

            var viewModel = new ValidateCATPageViewModel
            {
                FinacleData = finnacle,
                CatData = cat,
                Discrepancies = discrepancies
            };

            // Pass remarks to partial via ViewBag
            ViewBag.ExistingResolutions = savedRemarks;

            return PartialView("_DiscrepancyStatusPartial", viewModel);
        }
        private async Task<CMODashboardViewModel> GetCMODashboardData(string searchTerm, int page, string region, string status)
        {
            int pageSize = 10;
            IEnumerable<PendingConfirmedLoanViewModel> allLoans = new List<PendingConfirmedLoanViewModel>();
            allLoans = await GetPendingLoanFromStatus(status, region);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allLoans = allLoans.Where(l =>
                    l.accounT_NAME.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    l.id.ToString().Contains(searchTerm)
                ).ToList();
            }

            int totalItems = allLoans.Count();
            var pagedLoans = allLoans
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new CMODashboardViewModel
            {
                TotalUnreviewedLoans = await _dashboardapi.GetTotalPending(region),
                TotalReviewedLoans = await _dashboardapi.GetTotalConfirmed(region), 
                TotalUnresolvedLoans = await _dashboardapi.GetTotalRejected(region),
                TotalResolvedLoans = await _dashboardapi.GetTotalResolved(region),
                WithinTATUnreviewed = await _dashboardapi.GetWithinTATUnReview(region),
                WithinTATUnresolved = await _dashboardapi.GetWithinTATUnResolve(region),
                BeyondTATUnresolved = await _dashboardapi.GetBeyondTATUnResolve(region),
                BeyondTATUnreviewed = await _dashboardapi.GetBeyondTATUnReview(region),                

                loans = pagedLoans,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchTerm = searchTerm
            };
        }

        private async Task<CMSDashboardViewModel> GetCMSDashboardData(string searchTerm, int page, string region, string status)
        {
            int pageSize = 10;

            IEnumerable<PendingConfirmedLoanViewModel> allLoans = new List<PendingConfirmedLoanViewModel>();
            allLoans = await GetPendingLoanFromStatus(status, region);

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
            return new CMSDashboardViewModel
            {
                
                TotalApprovedLoans = await _dashboardapi.GetTotalApproved(region),
                TotalReviewedLoans = await _dashboardapi.GetTotalConfirmed(region),
                BeyondTATUnreviewed = await _dashboardapi.GetBeyondTATUnReview(region),
                BeyondTATReviewed = await _dashboardapi.GetBeyondTATUnApprove(region),

                // Table & Pagination data
                Confirmedloans = pagedLoans,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchTerm = searchTerm
            };
        }

        private async Task<AODashboardViewModel> GetAODashboardData(string searchTerm, int page, string region, string status)
        {
            int pageSize = 10;
            IEnumerable<PendingConfirmedLoanViewModel> allLoans = new List<PendingConfirmedLoanViewModel>();
            allLoans = await GetPendingLoanFromStatus(status, region);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allLoans = allLoans.Where(l =>
                    l.accounT_NAME.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    l.id.ToString().Contains(searchTerm)
                ).ToList();
            }

            int totalItems = allLoans.Count();
            var pagedLoans = allLoans
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new AODashboardViewModel
            {
                TotalUnresolvedLoans = await _dashboardapi.GetTotalRejected(region),
                TotalResolvedLoans = await _dashboardapi.GetTotalResolved(region),
                WithinTATUnresolved = await _dashboardapi.GetWithinTATUnResolve(region),
                BeyondTATUnresolved = await _dashboardapi.GetBeyondTATUnResolve(region),

                loans = pagedLoans,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchTerm = searchTerm
            };
        }
        private string GetMimeType(string ext)
        {
            return ext switch
            {
                "pdf" => "application/pdf",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "xls" => "application/vnd.ms-excel",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }

        private async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingLoanFromStatus(string status, string region)
        {
            IEnumerable<PendingConfirmedLoanViewModel> allLoans = new List<PendingConfirmedLoanViewModel>();
            if (status == "Unreviewed" || status == "")
            {
                allLoans = await _loanService.GetPendingConfirmedLoansByRegion(region);

            }
            else if (status == "Reviewed")
            {
                allLoans = await _loanService.GetConfirmedLoanByRegion(region);
            }
            else if (status == "Unresolved")
            {
                allLoans = await _loanService.GetRejectedLoanByRegion(region);
            }
            else if (status == "Approved")
            {
                allLoans = await _loanService.GetApprovedLoanByRegion(region);
            }
            else if (status == "Resolved")
            {
                allLoans = await _loanService.GetResolvedLoanByRegion(region);
            }
            else if (status == "WithinTATUnresolved")
            {
                var data = await _dashboardapi.GetWithinTATUnresolvedLoansByRegion(region);
                allLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(data);
            }
            else if (status == "BeyondTATUnreviewed")
            {
                var data = await _dashboardapi.GetBeyondTATUnreviewedLoansByRegion(region);
                allLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(data);
            }
            else if (status == "BeyondTATUnresolved")
            {
                var data = await _dashboardapi.GetBeyondTATUnresolvedLoansByRegion(region);
                allLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(data);
            }
            else if (status == "WithinTATUnreviewed")
            {
                var data = await _dashboardapi.GetWithinTATUnreviewedLoansByRegion(region);
                allLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(data);
            }
            return allLoans;
        }


    }
}
