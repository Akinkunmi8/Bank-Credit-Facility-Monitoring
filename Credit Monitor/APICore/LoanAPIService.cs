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
    }
}
