using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class TATService : ITAT
    {
        private readonly DALContext _context;
        private readonly ILogger<TATService> _logger;

        public TATService(DALContext context, ILogger<TATService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TATResponseDto> GetWithinTAT(string region)
        {
            try
            {
                // Defined as processed within the last 2 hours
                var threshold = DateTime.UtcNow.AddHours(-2);

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.PROCESSEDATE >= threshold && x.REGION == region)
                    .CountAsync();

                return new TATResponseDto
                {
                    IsSuccessful = true,
                    TATCount = count.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating 'Within TAT' count for region: {Region}", region);
                return new TATResponseDto { IsSuccessful = false, TATCount = "0" };
            }
        }

        public async Task<TATResponseDto> GetBeyondTAT(string region)
        {
            try
            {
                var threshold = DateTime.UtcNow.AddHours(-2);

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.PROCESSEDATE < threshold && x.CONFIRMED == false && x.REGION == region)
                    .CountAsync();

                return new TATResponseDto
                {
                    IsSuccessful = true,
                    TATCount = count.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating 'Beyond TAT' count for region: {Region}", region);
                return new TATResponseDto { IsSuccessful = false, TATCount = "0" };
            }
        }

        public async Task<List<Guid>> GetBeyondTATAppIds(string region)
        {
            _logger.LogDebug("Scanning database for applications beyond TAT in region: {Region}", region);

            try
            {
                var threshold = DateTime.UtcNow.AddHours(-2);

                var ids = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x =>
                        x.PROCESSEDATE < threshold &&
                        x.CONFIRMED == false &&
                        x.REGION == region)
                    .Select(x => x.Id)
                    .ToListAsync();

                if (ids.Any())
                {
                    _logger.LogInformation("Found {Count} applications beyond TAT in {Region}.", ids.Count, region);
                }

                return ids;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure retrieving 'Beyond TAT' IDs for {Region}", region);
                return new List<Guid>(); // Return empty list to prevent background worker crash
            }
        }
    }
}