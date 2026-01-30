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
        private readonly ILogger<AuditLookupService> _logger; // Added for better tracking

        public AuditLookupService(
            DALContext context,
            UserManager<AppUsers> userManager,
            ILogger<AuditLookupService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string?> GetUserEmailByAppIdAsync(Guid appId)
        {
            var audit = await _context.audits
                .Where(a => a.AppId == appId)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();

            if (audit == null)
            {
                _logger.LogWarning("Audit record not found for AppId: {AppId}", appId);
                return null; // Return null instead of throwing
            }

            var user = await _userManager.FindByIdAsync(audit.UserId);

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                _logger.LogWarning("Email not found for UserId: {UserId} linked to AppId: {AppId}", audit.UserId, appId);
                return null;
            }

            return user.Email;
        }

        public async Task<string?> GetUserEmailByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email; // Returns null naturally if user or email is missing
        }
    }

}
