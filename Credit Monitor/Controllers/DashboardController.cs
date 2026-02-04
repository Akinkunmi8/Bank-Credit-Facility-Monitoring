using System.Linq;
using System.Security.Claims;
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

        //private readonly string _storagePath = @"C:\Users\User\source\repos\Bank-Credit-Facility-Monitoring\BankLoanMonetrySolu\UploadedFiles";

        public DashboardController(
            ILoanService loanService,
            IDashboardAPI dashboardapi,
            IFileService fileService,
            ICovenantTrackerService covenantService,IConfiguration configuration) 
        {
            _loanService = loanService;
            _dashboardapi = dashboardapi;
            _fileService = fileService;
            _covenantService = covenantService;
            _storagePath = configuration["FileSettings:StoragePath"];
        }

        [Authorize(Roles = "staff")]
        public async Task<IActionResult> Staff()
        {
            var region = User.FindFirst("Region")?.Value;

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            var loan = await _loanService.GetPendingConfirmedLoansByRegion(region);
            return View(await GetStaffDashboardData(null, 1, region));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentStatus(Guid batchId)
        {
            if (batchId == Guid.Empty)
            {
                return PartialView("_DocumentStatusPartial", new List<DocumentDetailsViewModel>());
            }
            var documents = await _fileService.GetCustomerDocumentsAsync(batchId);
            return PartialView("_DocumentStatusPartial", documents);
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
        public async Task<IActionResult> FilterLoans(string searchTerm, string role, int page = 1)
        {
            var region = User.FindFirst("Region")?.Value;
            if (string.IsNullOrEmpty(region)) return Unauthorized();

            if(role == "supervisor")
            {
                var supervisorVM = await GetSupervisorDashboardData(searchTerm, page, region);
                return PartialView("_ConfirmedLoanTablePartial", supervisorVM);
            }

            var staffVM = await GetStaffDashboardData(searchTerm, page, region);
            return PartialView("_LoanTablePartial", staffVM);
        }


        [Authorize(Roles = "supervisor")]
        public async Task<IActionResult> Supervisor()
        {
            var region = User.FindFirst("Region")?.Value;

            if (string.IsNullOrEmpty(region)) return View(new List<PendingConfirmedLoanViewModel>());

            var loan = await _loanService.GetConfirmedLoanByRegion(region); //1.
            return View(await GetSupervisorDashboardData(null, 1, region));
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
        public async Task<IActionResult> UpdateStatus(Guid applicationId, string actionType, string returnUrl, string returnDashboard)
        {
            if (applicationId == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Invalid Application ID provided.";
                return Redirect(returnUrl);
            }

            var (isSuccess, message) = await _loanService.ProcessStatusUpdate(applicationId, actionType);

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

        private async Task<StaffDashboardViewModel> GetStaffDashboardData(string searchTerm, int page, string region)
        {
            int pageSize = 10;
            var allLoans = await _loanService.GetPendingConfirmedLoansByRegion(region);

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

            return new StaffDashboardViewModel
            {
                TotalPendingLoans = await _dashboardapi.GetTotalPending(region),
                TotalConfirmedLoans = await _dashboardapi.GetTotalConfirmed(region),
                TotalRejectedLoans = await _dashboardapi.GetTotalRejected(region),
                WithinTAT = await _dashboardapi.GetWithinTAT(region),
                BeyondTAT = await _dashboardapi.GetBeyondTAT(region),

                loans = pagedLoans,
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                SearchTerm = searchTerm
            };
        }

        private async Task<SupervisorDashboardViewModel> GetSupervisorDashboardData(string searchTerm, int page, string region)
        {
            int pageSize = 10;

            var allLoans = await _loanService.GetConfirmedLoanByRegion(region);

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
            return new SupervisorDashboardViewModel
            {
                
                TotalApprovedLoans = await _dashboardapi.GetTotalApproved(region),
                TotalConfirmedLoans = await _dashboardapi.GetTotalConfirmed(region),
                WithinTAT = await _dashboardapi.GetWithinTAT(region),
                BeyondTAT = await _dashboardapi.GetBeyondTAT(region),

                // Table & Pagination data
                Confirmedloans = pagedLoans,
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


    }
}
