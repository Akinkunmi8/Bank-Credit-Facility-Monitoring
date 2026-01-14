using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface IDashboardAPI
    {
        public Task<int> GetTotalPending(string region);
        public Task<int> GetTotalConfirmed(string region);
        public Task<int> GetTotalRejected(string region);
        public Task<int> GetWithinTAT(string region);
        public Task<int> GetBeyondTAT(string region);

    }
}
