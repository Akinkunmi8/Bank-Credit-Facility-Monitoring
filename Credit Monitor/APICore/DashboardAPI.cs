using Credit_Monitor.APIsDto;
using Credit_Monitor.Interfaces;
using Microsoft.AspNetCore.Identity.Data;

namespace Credit_Monitor.APICore
{
    public class DashboardAPI : IDashboardAPI
    {
        private readonly IBackendAPIService _apiService;

        public DashboardAPI(IBackendAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<int> GetTotalPending(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/pending?region={region}";

            var result = await _apiService.GetAsync<int>(urlPath);

            return result;
        }
        
        public async Task<int> GetTotalConfirmed(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/confirmed?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);
            return result;
        }

        public async Task<int> GetTotalRejected(string region)
        {
            var urlPath = $"/api/Dashboard/finacle/reconfirmed/{region}";
            var result = await _apiService.GetAsync<int>(urlPath);
            return result;
        }
       
        public async Task<int> GetTotalApproved(string region)
        {
            var urlPath = $"api/Dashboard/finacle/approved?region={region}";
            var result = await _apiService.GetAsync<int>(urlPath);
            return result;
        }
        public async Task<int> GetWithinTAT(string region)
        {
            var urlPath = $"/api/Dashboard/tat/within/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);
            if(!result.isSuccessful)
            {
                return 0;
            }
            return Int32.Parse(result.tatCount);
            
        }
        public async Task<int> GetBeyondTAT(string region)
        {
            var urlPath = $"/api/Dashboard/tat/beyond/{region}";
            var result = await _apiService.GetAsync<TATResponseDto>(urlPath);
            if (!result.isSuccessful)
            {
                return 0;
            }
            return Int32.Parse(result.tatCount);

        }
        
        
        
        
    }
}
