using System.Threading.Tasks;
using AutoMapper;
using Credit_Monitor.APICore;
using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly ILoanAPIService _loanAPIservice;
        private readonly ICatAPIService _catAPIservice;
        private readonly ILogger<LoanService> _logger;

        public LoanService(
            ILoanRepository loanRepository,
            IMapper mapper,
            ILoanAPIService loanAPIservice,
            ICatAPIService catAPIservice,
            ILogger<LoanService> logger) // 2. Inject logger
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _loanAPIservice = loanAPIservice;
            _catAPIservice = catAPIservice;
            _logger = logger;
        }
        public async Task<PendingLoanWithCustomerDetailsViewModel?> GetPendingConfirmedLoanByIdAsync(string id)
        {
            _logger.LogInformation("Fetching pending confirmed loan details for ID: {LoanId}", id);

            try
            {
                var loan = await _loanAPIservice.GetPendingLoanById(id);

                if (loan == null)
                {
                    _logger.LogWarning("No pending loan found for ID: {LoanId}", id);
                    return null;
                }

                var loanVM = _mapper.Map<PendingConfirmedLoanViewModel>(loan);
                var customerDetails = _mapper.Map<CustomerDetailsViewModel>(loan.customer);

                _logger.LogInformation("Successfully retrieved and mapped loan/customer details for ID: {LoanId}", id);

                return new PendingLoanWithCustomerDetailsViewModel
                {
                    loan = loanVM,
                    customer = customerDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving pending loan ID: {LoanId}", id);
                throw; 
            }
        }
        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region)
        {
            _logger.LogInformation("Fetching pending confirmed loans for region: {Region}", region);

            try
            {
                var loans = await _loanAPIservice.GetPendingLoansByRegion(region);

                if (loans == null)
                {
                    _logger.LogWarning("API returned null for pending loans in region: {Region}. Returning empty list.", region);
                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mappedLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);

                _logger.LogInformation("Successfully retrieved {Count} pending confirmed loans for region: {Region}",
                    mappedLoans.Count(), region);

                return mappedLoans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve pending confirmed loans for region: {Region}", region);
                throw;
            }
        }
        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetConfirmedLoanByRegion(string region)
        {
            _logger.LogInformation("Requesting all CONFIRMED loans for region: {Region}", region);

            try
            {
                var loans = await _loanAPIservice.GetConfirmedLoansByRegion(region);

                if (loans == null)
                {
                    _logger.LogWarning("No confirmed loan data returned from API for region: {Region}. Returning empty collection.", region);
                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mappedLoans = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);

                _logger.LogInformation("Successfully mapped {Count} confirmed loans for region: {Region}",
                    mappedLoans.Count(), region);

                return mappedLoans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching confirmed loans for region: {Region}", region);
                throw;
            }
        }

        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetApprovedLoanByRegion(string region)
        {
            _logger.LogInformation("Requesting APPROVED loans for region: {Region}", region);

            try
            {
                var loans = await _loanAPIservice.GetApprovedLoansByRegion(region);

                if (loans == null)
                {
                    _logger.LogWarning("No approved loans found in API response for region: {Region}", region);
                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mapped = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);
                _logger.LogInformation("Retrieved {Count} approved loans for region: {Region}", mapped.Count(), region);

                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching approved loans for region: {Region}", region);
                throw;
            }
        }

        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetResolvedLoanByRegion(string region)
        {
            //_logger.LogInformation("Requesting RESOLVED loans for region: {Region}", region);

            try
            {
                var loans = await _loanAPIservice.GetResolvedLoansByRegion(region);

                if (loans == null)
                {
                  //  _logger.LogWarning("No resolved loans found in API response for region: {Region}", region);
                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mapped = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);
                //_logger.LogInformation("Retrieved {Count} resolved loans for region: {Region}", mapped.Count(), region);

                return mapped;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error fetching resolved loans for region: {Region}", region);
                throw;
            }
        }

        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetRejectedLoanByRegion(string region)
        {
            _logger.LogInformation("Requesting REJECTED loans for region: {Region}", region);

            try
            {
                var loans = await _loanAPIservice.GetRejectedLoansByRegion(region);

                if (loans == null)
                {
                    _logger.LogWarning("No rejected loans found in API response for region: {Region}", region);
                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mapped = _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);
                _logger.LogInformation("Retrieved {Count} rejected loans for region: {Region}", mapped.Count(), region);

                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching rejected loans for region: {Region}", region);
                throw;
            }
        }

        public async Task<FinacleDataViewModel?> GetPendingConfirmedLoanByAccNumAsync(string AccNum)
        {
            _logger.LogInformation("Searching for pending loan by Account Number: {AccNum}", AccNum);

            try
            {
                var loan = await _loanAPIservice.GetPendingLoanByAccountNumber(AccNum);

                if (loan == null)
                {
                    _logger.LogWarning("Account Number {AccNum} not found in pending loans.", AccNum);
                    return null;
                }

                var result = _mapper.Map<FinacleDataViewModel>(loan);
                _logger.LogInformation("Successfully retrieved Finacle data for AccNum: {AccNum}", AccNum);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending loan for AccNum: {AccNum}", AccNum);
                throw;
            }
        }

        public async Task<CATViewModel> GetCustomerLoanDetailsfromCATAsync(string AccNum)
        {
            _logger.LogInformation("Attempting to fetch Customer Loan Details from CAT API for Account: {AccNum}", AccNum);

            try
            {
                var data = await _catAPIservice.GetCustomerLoanDetailsfromCAT(AccNum);

                if (data == null)
                {
                    _logger.LogWarning("CAT API returned no data for Account: {AccNum}", AccNum);
                    return new CATViewModel(); 
                }

                var mappedData = _mapper.Map<CATViewModel>(data);

                _logger.LogInformation("Successfully retrieved and mapped CAT data for Account: {AccNum}", AccNum);

                return mappedData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calling CAT API for Account: {AccNum}", AccNum);
                throw;
            }
        }

        public async Task<DiscrepanciesViewModel> ValidateLoanDiscrepancies(string AccNum)
        {
            _logger.LogInformation("Starting Cross-System Validation for Account: {AccNum}", AccNum);

            try
            {
                // 1. Fetch from both systems
                var finnacle = await _loanAPIservice.GetPendingLoanByAccountNumber(AccNum);
                var cat = await _catAPIservice.GetCustomerLoanDetailsfromCAT(AccNum);

                var result = new DiscrepanciesViewModel { AccNum = AccNum };

                // 2. Log if data is missing from one of the systems
                if (finnacle == null || cat == null)
                {
                    _logger.LogWarning("Validation aborted for {AccNum}. Missing Data - Finacle: {FinacleStatus}, CAT: {CatStatus}",
                        AccNum,
                        finnacle == null ? "NOT FOUND" : "FOUND",
                        cat == null ? "NOT FOUND" : "FOUND");
                    return result;
                }

                // 3. Perform Comparisons
                result.CustomerName = CompareStrings(finnacle.AccountName, cat.CustomerName, "Customer Name");
                result.Directorate = CompareStrings(finnacle.SbuName, cat.Directorate, "Directorate/SBU");
                result.FacilityType = CompareStrings(finnacle.FacilityType, cat.FacilityType, "Facility Type");
                result.Fee = CompareDecimals(finnacle.FxRate, cat.Fees, "Fee");
                result.ApprovalLimit = CompareDecimals(finnacle.SanctionLimit, cat.ApprovalLimit, "Limit");

                decimal FinnacleTotalBalance = finnacle.TotalBalanceLcy != 0
                                                ? finnacle.TotalBalanceLcy
                                                : finnacle.TotalBalanceFcy;

                result.DisbursmentRequired = CompareDecimals(FinnacleTotalBalance, cat.DisbursementRequired, "Disbursed Amount");
                result.Interest = CompareDecimals(finnacle.IntRate, cat.Interest, "Interest Rate", isPercentage: true);
                result.MaturityDate = CompareDates(finnacle.ExpiryDate, cat.MaturityOrExpiry, "Maturity Date");
                result.ValueDate = CompareDates(finnacle.DateGranted, cat.ValueDate, "Value Date");

                // 4. Summarize Results
                var allResults = new[] {
            result.CustomerName, result.ApprovalLimit, result.FacilityType,
            result.Interest, result.MaturityDate, result.Directorate,
            result.DisbursmentRequired, result.Fee, result.ValueDate
        };

                result.ErrorCount = allResults.Count(x => !x.IsValid);
                result.TotalFields = allResults.Length;

                // 5. Final Audit Log
                if (result.ErrorCount > 0)
                {
                    _logger.LogWarning("Validation Completed for {AccNum}: FOUND {ErrorCount} DISCREPANCIES out of {TotalCount} fields.",
                        AccNum, result.ErrorCount, result.TotalFields);
                }
                else
                {
                    _logger.LogInformation("Validation Completed for {AccNum}: 100% Match across all systems.", AccNum);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure during validation logic for Account: {AccNum}", AccNum);
                throw;
            }
        }

        public async Task<(bool isSuccess, string message)> ProcessStatusUpdate(Guid appId, string actionType, DateTime? deadline)
        {
            // 1. Log the intent with structured properties
            _logger.LogInformation("Initiating status update. Action: {ActionType}, ApplicationId: {AppId}", actionType, appId);

            try
            {
                var request = new StatusUpdateRequestDto
                {
                    ApplicationId = appId.ToString(),
                    Action = actionType,
                    deadline = deadline
                };

                // 2. Call the API and capture the response
                var response = await _loanAPIservice.UpdateLoanStatusAsync(request);

                if (response != null && response.Success)
                {
                    // 3. Log the successful state change
                    _logger.LogInformation("Successfully updated status to {ActionType} for ApplicationId: {AppId}. API Message: {Message}",
                        actionType, appId, response.Message);

                    return (true, response.Message);
                }

                // 4. Log a warning for business-level failures (e.g., "Insufficient permissions" or "Invalid state")
                _logger.LogWarning("API rejected status update ({ActionType}) for ApplicationId: {AppId}. Reason: {Reason}",
                    actionType, appId, response?.Message ?? "No message provided by API");

                return (false, response?.Message ?? "Failed to update status via API");
            }
            catch (Exception ex)
            {
                // 5. Critical error (e.g., API is down, network timeout)
                _logger.LogError(ex, "Critical failure while attempting to update status ({ActionType}) for ApplicationId: {AppId}",
                    actionType, appId);

                return (false, $"Internal system error: {ex.Message}");
            }
        }

        public async Task<List<ResolutionItem>> GetSavedResolutionsMapped(Guid? appId)
        {
            if (!appId.HasValue || appId.Value == Guid.Empty)
                return new List<ResolutionItem>();

            var remarks =
                await _loanAPIservice.GetExistingRemarksByAppId(
                    appId.Value.ToString());

            if (remarks == null)
                return new List<ResolutionItem>();

            return remarks.Select(x => new ResolutionItem
            {
                Id = x.Id.GetValueOrDefault(),

                Discrepancy = x.Discrepancies,

                // CMO
                RecommendedAction = x.RecomendedAction,
                Remarks = x.Remarks,
                Responsibility = x.Responsibility,

                // RM/AO
                ResolutionAction = x.ResolutionAction,
                ResolutionRemarks = x.ResolutionRemarks,
                RespondedBy = x.RespondedBy,
                RespondedAt = x.RespondedAt

            }).ToList();
        }
        public async Task<IEnumerable<PendingConfirmedLoanViewModel>>GetCMOReviewLoanByRegion(string region)
        {
            _logger.LogInformation(
                "Requesting CMO resolution review queue for Region: {Region}",
                region);

            try
            {
                var loans =
                    await _loanAPIservice.GetCMOReviewByRegion(region);

                if (loans == null)
                {
                    _logger.LogWarning(
                        "No CMO review records returned for Region: {Region}",
                        region);

                    return Enumerable.Empty<PendingConfirmedLoanViewModel>();
                }

                var mappedLoans =
                    _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);

                _logger.LogInformation(
                    "Retrieved {Count} CMO review record(s) for Region: {Region}",
                    mappedLoans.Count(),
                    region);

                return mappedLoans;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving CMO review queue for Region: {Region}",
                    region);

                throw;
            }
        }
        public async Task<(int SuccessCount, int FailureCount)> ProcessBulkResolutionsAsync(Guid appId, List<ResolutionItem> resolutions)
        {
            int successCount = 0;
            int failureCount = 0;

            if (resolutions == null || !resolutions.Any()) return (0, 0);

            foreach (var item in resolutions)
            {
                // Rule: Only process if a recommendation was provided
                if (string.IsNullOrWhiteSpace(item.RecommendedAction)) continue;

                var requestDto = new SaveRemarkRequestDto
                {
                    Id = item.Id,
                    AppId = appId,
                    Discrepancies = item.Discrepancy,
                    Remarks = item.Remarks ?? "N/A",
                    RecomendedAction = item.RecommendedAction,
                    Responsibility = item.Responsibility ?? "Not Assigned"
                };

                try
                {
                    // Calling the API logic we built earlier
                    var response = await _loanAPIservice.SaveDiscrepancyRemarksAsync(requestDto);

                    if (response != null) // If your API returns a boolean or success property, check it here
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                        _logger.LogWarning("Bulk Resolution: Failed to save {Field} for AppId {AppId}", item.FieldName, appId);
                    }
                }
                catch (Exception ex)
                {
                    failureCount++;
                    _logger.LogError(ex, "Bulk Resolution: Exception saving {Field}", item.FieldName);
                }
            }

            return (successCount, failureCount);
        }

        public async Task<(int SuccessCount, int FailureCount)>ProcessBulkResolutionResponsesAsync(
        Guid appId,
        List<ResolutionItem> resolutions)
        {
            int successCount = 0;
            int failureCount = 0;

            if (resolutions == null || !resolutions.Any())
                return (0, 0);

            foreach (var item in resolutions)
            {
                // AO/RM must provide corrective action
                if (string.IsNullOrWhiteSpace(item.ResolutionAction))
                {
                    failureCount++;

                    _logger.LogWarning(
                        "Resolution response missing for discrepancy {Field} - AppId {AppId}",
                        item.FieldName,
                        appId);

                    continue;
                }

                // Existing discrepancy record must exist
                if (item.Id == Guid.Empty)
                {
                    failureCount++;

                    _logger.LogWarning(
                        "Resolution response has no discrepancy Id for {Field} - AppId {AppId}",
                        item.FieldName,
                        appId);

                    continue;
                }

                var request = new SubmitDiscrepancyResponseDto
                {
                    Id = item.Id,
                    AppId = appId,
                    ResolutionAction = item.ResolutionAction.Trim(),
                    ResolutionRemarks = item.ResolutionRemarks?.Trim()
                };

                try
                {
                    var response =
                        await _loanAPIservice
                            .SubmitDiscrepancyResponseAsync(request);

                    if (response != null && response.IsSuccess)
                    {
                        successCount++;
                    }
                    else
                    {
                        failureCount++;

                        _logger.LogWarning(
                            "Failed saving AO/RM response for {Field}. AppId {AppId}. Message: {Message}",
                            item.FieldName,
                            appId,
                            response?.ResponseMessage);
                    }
                }
                catch (Exception ex)
                {
                    failureCount++;

                    _logger.LogError(
                        ex,
                        "Exception saving AO/RM response for {Field}. AppId {AppId}",
                        item.FieldName,
                        appId);
                }
            }

            return (successCount, failureCount);
        }

        public async Task<ServiceResponseDto>SubmitResolutionForReviewAsync(Guid appId)
        {
            if (appId == Guid.Empty)
            {
                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Invalid loan application."
                };
            }

            try
            {
                var request = new SubmitResolutionForReviewDto
                {
                    AppId = appId
                };

                var response =
                    await _loanAPIservice
                        .SubmitResolutionForReviewAsync(request);

                if (response == null)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage =
                            "No response was received while submitting the application for CMO review."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error submitting resolution application for CMO review. AppId {AppId}",
                    appId);

                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage =
                        "Unable to submit the application for CMO review."
                };
            }
        }

        public async Task<FullDashboardReportDataDto?> GetFullReportPreview(string region,string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                    return null;

                if (string.IsNullOrWhiteSpace(role))
                    return null;

                return await _loanAPIservice
                    .GetFullReportPreview(region, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving full report preview. Region: {Region}, Role: {Role}",
                    region,
                    role);

                return null;
            }
        }
        private ValidationResult CompareStrings(string fin, string cat, string fieldName)
        {
            bool match = string.Equals(fin?.Trim(), cat?.Trim(), StringComparison.OrdinalIgnoreCase);
            return new ValidationResult
            {
                IsValid = match,
                Message = match ? "Match" : $"{fieldName} mismatch: Finacle shows '{fin}', CAT shows '{cat}'"
            };
        }

        private ValidationResult CompareDecimals(decimal fin, decimal cat, string fieldName, bool isPercentage = false)
        {
            decimal diff = fin - cat;
            bool match = Math.Abs(diff) < 0.01m; // Using a small epsilon for decimal precision

            string suffix = isPercentage ? "%" : "";
            return new ValidationResult
            {
                IsValid = match,
                Message = match ? "Match" : $"{fieldName} mismatch of {Math.Abs(diff):N2}{suffix}",
                DiscrepancyAmount = Math.Abs(diff).ToString("N2")
            };
        }

        private ValidationResult CompareDates(DateTime? fin, DateTime? cat, string fieldName)
        {
            bool match = fin?.Date == cat?.Date;
            return new ValidationResult
            {
                IsValid = match,
                Message = match ? "Match" : $"{fieldName} mismatch: {fin?.ToString("dd/MM/yyyy")} vs {cat?.ToString("dd/MM/yyyy")}"
            };
        }
    }
}
