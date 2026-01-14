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
        public LoanService(ILoanRepository loanRepository, IMapper mapper, ILoanAPIService loanAPIservice)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _loanAPIservice = loanAPIservice;
        }
        public async Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByIdAsync(string id)
        {
            var loan = await _loanRepository.GetPendingConfirmedLoanByIdAsync(id);
            if (loan == null) return null;
            return _mapper.Map<PendingConfirmedLoanViewModel>(loan);
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

        public async Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByAccNumAsync(string AccNum)
        {
            var loan = await _loanRepository.GetPendingConfirmedLoanByAccNumAsync(AccNum);
            return _mapper.Map<PendingConfirmedLoanViewModel>(loan);
        }

        public async Task<CATViewModel> GetCustomerLoanDetailsfromCATAsync(string AccNum)
        {
            var data = await _loanRepository.GetCustomerLoanDetailsfromCAT(AccNum);
            return _mapper.Map<CATViewModel>(data);   
        }

        public async Task<DiscrepanciesViewModel> ValidateLoanDiscrepancies(string AccNum)
        {
            var finnacle = await _loanRepository.GetPendingConfirmedLoanByAccNumAsync(AccNum);
            var cat = await _loanRepository.GetCustomerLoanDetailsfromCAT(AccNum);

            var result = new DiscrepanciesViewModel { AccNum = AccNum };

            if (finnacle == null || cat == null) return result;

            result.CustomerName = CompareStrings(finnacle.accounT_NAME, cat.customerName, "Customer Name");

            result.Directorate = CompareStrings(finnacle.sbU_NAME, cat.directorate, "Directorate/SBU");

            result.FacilityType = CompareStrings(finnacle.facilitY_TYPE, cat.facilityType, "Facility Type");

            result.Fee = CompareDecimals(finnacle.fX_RATE, cat.fees, "Fee");
           
            result.ApprovalLimit = CompareDecimals(finnacle.sanctioN_LIMIT, cat.approvalLimit, "Limit");

            decimal FinnacleTotalBalance = finnacle.totaL_BALANCE_LCY != 0
                                           ? finnacle.totaL_BALANCE_LCY
                                           : finnacle.totaL_BALANCE_FCY;

            result.DisbursmentRequired = CompareDecimals(FinnacleTotalBalance, cat.disbursementRequired, "Disbursed Amount");

            result.Interest = CompareDecimals(finnacle.inT_RATE, cat.interest, "Interest Rate", isPercentage: true);

            result.MaturityDate = CompareDates(finnacle.expirY_DATE, cat.maturityOrExpiry, "Maturity Date");

            result.ValueDate = CompareDates(finnacle.datE_GRANTED, cat.valuedate, "Value Date");



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
