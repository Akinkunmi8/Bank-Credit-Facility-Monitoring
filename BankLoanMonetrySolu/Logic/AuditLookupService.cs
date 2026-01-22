using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankLoanMonetrySolu.Logic
{
    public class AuditLookupService : IAuditLookupService
    {
        private readonly DALContext _context;
        private readonly UserManager<AppUsers> _userManager;

        public AuditLookupService(
            DALContext context,
            UserManager<AppUsers> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> GetUserEmailByAppIdAsync(Guid appId)
        {
            var audit = await _context.audits
                .Where(a => a.AppId == appId)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();

            if (audit == null)
                throw new Exception("Audit record not found");

            var user = await _userManager.FindByIdAsync(audit.UserId);
            return user?.Email ?? throw new Exception("User email not found");
        }

        public async Task<string> GetUserEmailByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email ?? throw new Exception("User email not found");
        }
    }

}
