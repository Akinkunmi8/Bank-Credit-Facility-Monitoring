using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Logic
{
    public class AuditLookupService : IAuditLookupService
    {
        private readonly DALContext _context;
        private readonly UserManager<AppUsers> _userManager;
        private readonly ILogger<AuditLookupService> _logger;

        public AuditLookupService(
            DALContext context,
            UserManager<AppUsers> userManager,
            ILogger<AuditLookupService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Finds the email of the user associated with the most recent audit entry for a specific application.
        /// </summary>
        public async Task<string?> GetUserEmailByAppIdAsync(Guid appId)
        {
            _logger.LogDebug("Looking up audit trail for AppId: {AppId}", appId);

            try
            {
                // Fetch the most recent audit entry to find the current 'owner' or 'actor'
                var audit = await _context.audits
                    .Where(a => a.AppId == appId)
                    .OrderByDescending(a => a.CreatedAt)
                    .FirstOrDefaultAsync();

                if (audit == null)
                {
                    _logger.LogWarning("No audit trail exists for AppId: {AppId}. Cannot resolve recipient email.", appId);
                    return null;
                }

                if (string.IsNullOrEmpty(audit.UserId))
                {
                    _logger.LogWarning("Audit record found for AppId: {AppId}, but UserId is null or empty.", appId);
                    return null;
                }

                var user = await _userManager.FindByIdAsync(audit.UserId);

                if (user == null)
                {
                    _logger.LogError("Audit record exists for UserId {UserId}, but user does not exist in Identity Store. AppId: {AppId}", audit.UserId, appId);
                    return null;
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    _logger.LogWarning("User {UserId} found, but has no email address configured.", audit.UserId);
                    return null;
                }

                return user.Email;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while looking up user email for AppId: {AppId}", appId);
                return null;
            }
        }

        /// <summary>
        /// Directly resolves a User ID to an Email address.
        /// </summary>
        public async Task<string?> GetUserEmailByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("GetUserEmailByUserIdAsync called with null or empty UserId.");
                return null;
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("Identity lookup failed: User {UserId} not found.", userId);
                    return null;
                }

                return user.Email;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching email for UserId: {UserId}", userId);
                return null;
            }
        }
    }
}