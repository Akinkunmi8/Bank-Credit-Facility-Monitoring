using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;

namespace Credit_Monitor.APICore
{
    public class LoanAPIService : ILoanAPIService
    {
        private readonly IBackendAPIService _apiService;

        public LoanAPIService(IBackendAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<List<FinacleRecord>?> GetPendingLoansByRegion(string region)
        {
            var urlPath = $"/api/Finacle/by-region/{region}";
            var result = await _apiService.GetAsync<FinacleResponseWrapper>(urlPath);
            if (result == null ||  !result.isSuccess)
            {
                return null;
            }
            return result.dataList ?? [];
        }
       
        public async Task<FinacleLoanAndCustomerDto?> GetPendingLoanById(string id)
        {
            var urlPath = $"api/Dashboard/View-LoanAndCustomerDetails?appId={id}";
            var result = await _apiService.GetAsync<FinacleSingleResponseWrapper>(urlPath);
            if (result == null || !result.isSuccess)
            {
                return null;
            }
            
            return result.data;
        }

        public async Task<FinacleDataResponseDto?> GetPendingLoanByAccountNumber(string AccNum)
        {
            var urlPath = $"api/Finacle/CompareData?acctNum={AccNum}";
            var result = await _apiService.GetAsync<FinacleDataResponseDto>(urlPath);
            if (result == null || !result.IsSuccess)
            {
                return null;
            }
            return result;
        }
    }
}
