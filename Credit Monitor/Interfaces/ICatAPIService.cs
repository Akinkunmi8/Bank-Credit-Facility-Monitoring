using Credit_Monitor.APIDto;

namespace Credit_Monitor.Interfaces
{
    public interface ICatAPIService
    {
        public Task<CatDataDto?> GetCustomerLoanDetailsfromCAT(string AccNum);
    }
}
