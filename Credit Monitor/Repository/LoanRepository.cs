using Credit_Monitor.Data;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Microsoft.EntityFrameworkCore;

namespace Credit_Monitor.Repository
{
    public class LoanRepository : ILoanRepository
    {
        public async Task<PendingConfirmedLoan?> GetPendingConfirmedLoanByIdAsync(string id)
        {
            return await Task.FromResult(InMemoryLoans.Loans.FirstOrDefault(l => l.id == id));
        }

        public async Task<IEnumerable<PendingConfirmedLoan>> GetPendingConfirmedLoansByRegionAsync(string region)
        {
            var loans = InMemoryLoans.Loans
                .Where(l => l.regioN_NAME != null && l.regioN_NAME.Equals(region, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return await Task.FromResult(loans);
        }
    }
}
