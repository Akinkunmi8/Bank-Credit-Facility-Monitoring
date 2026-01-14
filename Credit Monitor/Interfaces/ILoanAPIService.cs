using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface ILoanAPIService
    {
        public Task<List<FinacleRecord>?> GetPendingLoansByRegion(string region);
    }
}
