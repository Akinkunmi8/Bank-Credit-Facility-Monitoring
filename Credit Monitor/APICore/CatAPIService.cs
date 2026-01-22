using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;

namespace Credit_Monitor.APICore
{
    public class CatAPIService : ICatAPIService
    {
        private readonly IBackendAPIService _apiService;

        public CatAPIService(IBackendAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<CatDataDto?> GetCustomerLoanDetailsfromCAT(string AccNum)
        {

            var urlPath = $"/api/CAT/GetByAccount/{AccNum}";
            var result = await _apiService.GetAsync<CatResponseDto>(urlPath);
            if (result == null || !result.IsSuccess)
            {
                return null;
            }
            return result.Data;
        
        }
    }
}
