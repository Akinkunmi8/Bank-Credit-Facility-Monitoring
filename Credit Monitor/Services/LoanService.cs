using AutoMapper;
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
        public LoanService(ILoanRepository loanRepository, IMapper mapper, ILoanAPIService loanAPIservice, ICatAPIService catAPIservice)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _loanAPIservice = loanAPIservice;
            _catAPIservice = catAPIservice;
        }
        public async Task<PendingLoanWithCustomerDetailsViewModel?> GetPendingConfirmedLoanByIdAsync(string id)
        {
            var loan = await _loanAPIservice.GetPendingLoanById(id);
            if (loan == null) return null;
            var loanVM = _mapper.Map<PendingConfirmedLoanViewModel>(loan);
            var customerDetails = _mapper.Map<CustomerDetailsViewModel>(loan.customer);
            return new PendingLoanWithCustomerDetailsViewModel
            {
                loan = loanVM,
                customer = customerDetails
            };
        }

        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region)
        {
            var loans = await _loanAPIservice.GetPendingLoansByRegion(region);
            if (loans == null)
            {
                return Enumerable.Empty<PendingConfirmedLoanViewModel>();
            }
            return _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);
        }

        public async Task<FinacleDataViewModel?> GetPendingConfirmedLoanByAccNumAsync(string AccNum)
        {
            var loan = await _loanAPIservice.GetPendingLoanByAccountNumber(AccNum);
            return _mapper.Map<FinacleDataViewModel>(loan);
        }

        public async Task<CATViewModel> GetCustomerLoanDetailsfromCATAsync(string AccNum)
        {
            var data = await _catAPIservice.GetCustomerLoanDetailsfromCAT(AccNum);
            return _mapper.Map<CATViewModel>(data);   
        }

        public async Task<DiscrepanciesViewModel> ValidateLoanDiscrepancies(string AccNum)
        {
            var finnacle = await _loanAPIservice.GetPendingLoanByAccountNumber(AccNum);
            var cat = await _catAPIservice.GetCustomerLoanDetailsfromCAT(AccNum);

            var result = new DiscrepanciesViewModel { AccNum = AccNum };

            if (finnacle == null || cat == null) return result;

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



            // Calculate Totals for the Dashboard counter
            var allResults = new[] { 
                result.CustomerName,
                result.ApprovalLimit,
                result.FacilityType,
                result.Interest, 
                result.MaturityDate,
                result.Directorate,
                result.DisbursmentRequired,
                result.Fee,
                result.ValueDate
            };
            result.ErrorCount = allResults.Count(x => !x.IsValid);
            result.TotalFields = allResults.Length;

            return result;
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
