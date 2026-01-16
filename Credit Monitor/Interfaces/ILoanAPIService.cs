using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanAPIService
    {
        public Task<List<FinacleRecord>?> GetPendingLoansByRegion(string region);
        public Task<FinacleLoanAndCustomerDto?> GetPendingLoanById(string id);
        public Task<FinacleDataResponseDto?> GetPendingLoanByAccountNumber(string AccNum);
    }
}
