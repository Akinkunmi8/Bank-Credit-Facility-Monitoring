
using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;

namespace Credit_Monitor.Interfaces
{
    public interface IDashboardAPI
    {
        public Task<int> GetTotalPending(string region);
        public Task<int> GetTotalConfirmed(string region);
        public Task<int> GetTotalRejected(string region);
        public Task<int> GetTotalApproved(string region);
        public Task<int> GetTotalResolved(string region);
        public Task<int> GetWithinTAT(string region);
        public Task<int> GetWithinTATUnReview(string region);
        public Task<int> GetWithinTATUnResolve(string region);
        public Task<int> GetBeyondTATUnReview(string region);
        public Task<int> GetBeyondTATUnApprove(string region);
        public Task<int> GetBeyondTATUnResolve(string region);
        public Task<List<TATOutputResponse>?> GetWithinTATUnresolvedLoansByRegion(string region);
        public Task<List<TATOutputResponse>?> GetBeyondTATUnreviewedLoansByRegion(string region);
        public Task<List<TATOutputResponse>?> GetBeyondTATUnresolvedLoansByRegion(string region);
        public Task<List<TATOutputResponse>?> GetWithinTATUnreviewedLoansByRegion(string region);
        public Task<string?> GetDiscrepancyRemarks(Guid appId);
        public Task<int> GetBeyondTAT(string region);

    }
}
