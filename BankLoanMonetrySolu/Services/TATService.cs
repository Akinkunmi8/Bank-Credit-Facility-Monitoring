using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
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
        private readonly IMapper _mapper;

        public TATService(DALContext context, ILogger<TATService> logger,IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TATResponseDto> GetWithinTATForUnReview(string region)
        {
            try
            {
                // Defined as processed within the last 2 hours
                var threshold = DateTime.UtcNow.AddHours(-2);

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.PROCESSEDATE >= threshold && x.CONFIRMED == false && x.RECONFIRMED == false && x.APPROVED == false && x.ISRESOLVED == false && x.REGION == region)
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

        public async Task<TATResponseDto> GetWithinTATForUnResolved(string region)
        {
            try
            {
                var now = DateTime.UtcNow;

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.DEADLINE >= now && // Deadline is still in the future
                                x.ISRESOLVED == false &&
                                x.REGION == region)
                    .CountAsync();

                return new TATResponseDto { IsSuccessful = true, TATCount = count.ToString() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating 'Within TAT for Unresolved' for region: {Region}", region);
                return new TATResponseDto { IsSuccessful = false, TATCount = "0" };
            }
        }
        public async Task<TATResponseDto> GetBeyondTATForUnreview(string region)
        {
            try
            {
                var threshold = DateTime.UtcNow.AddHours(-2);

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.PROCESSEDATE < threshold && x.CONFIRMED == false && x.RECONFIRMED == false && x.APPROVED == false && x.ISRESOLVED == false && x.REGION == region)
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

        public async Task<TATResponseDto> GetBeyondTATForUnResolved(string region)
        {
            try
            {
                var now = DateTime.UtcNow;

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.DEADLINE < now && // Deadline has passed
                                x.ISRESOLVED == false &&
                                x.REGION == region)
                    .CountAsync();

                return new TATResponseDto { IsSuccessful = true, TATCount = count.ToString() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating 'Beyond TAT for Unresolved' for region: {Region}", region);
                return new TATResponseDto { IsSuccessful = false, TATCount = "0" };
            }
        }

        public async Task<TATResponseDto> GetBeyondTATForReviewedforApproval(string region)
        {
            try
            {
                var threshold = DateTime.UtcNow.AddHours(-2);

                var count = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(x => x.PROCESSEDATE < threshold && x.CONFIRMED == true && x.APPROVED == false && x.REGION == region)
                    .CountAsync();

                return new TATResponseDto
                {
                    IsSuccessful = true,
                    TATCount = count.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating 'Beyond TAT for Reviwed for Approval' count for region: {Region}", region);
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

        // Returns Unreviewed data within 2-hour window
        public async Task<List<FinacleDataDto>> GetDataWithinTATForUnReview(string region)
        {
            var threshold = DateTime.UtcNow.AddHours(-2);
            var data = await _context.finacleDatas
                .AsNoTracking()
                .Where(x => x.PROCESSEDATE >= threshold &&
                            x.CONFIRMED == false &&
                            x.RECONFIRMED == false &&
                            x.APPROVED == false &&
                            x.ISRESOLVED == false &&
                            x.REGION == region)
                .ToListAsync();

            return _mapper.Map<List<FinacleDataDto>>(data);
        }

        // Returns Unreviewed data where more than 2 hours have passed
        public async Task<List<FinacleDataDto>> GetDataBeyondTATForUnReview(string region)
        {
            var threshold = DateTime.UtcNow.AddHours(-2);
            var data = await _context.finacleDatas
                .AsNoTracking()
                .Where(x => x.PROCESSEDATE < threshold &&
                            x.CONFIRMED == false &&
                            x.RECONFIRMED == false &&
                            x.APPROVED == false &&
                            x.ISRESOLVED == false &&
                            x.REGION == region)
                .ToListAsync();

            return _mapper.Map<List<FinacleDataDto>>(data);
        }
        public async Task<List<UnresolvedTATDetailDto>> GetDataWithinTATForUnResolved(string region)
        {
            var now = DateTime.UtcNow;
            try
            {
                var queryData = await (from finacle in _context.finacleDatas.AsNoTracking()
                                       join remark in _context.discrepancyRemarks.AsNoTracking()
                                       on finacle.Id equals remark.AppId
                                       where finacle.REGION == region &&
                                             finacle.ISRESOLVED == false &&
                                             finacle.DEADLINE >= now
                                       select new { finacle, remark })
                                       .ToListAsync();

                // Map explicitly here to avoid Type Conversion errors
                return queryData.Select(x => MapToSingleDto(x.finacle, x.remark)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Joined Within TAT Unresolved for {Region}", region);
                return new List<UnresolvedTATDetailDto>();
            }
        }

        // Returns Unresolved data where the Deadline HAS passed (BEYOND TAT)
        public async Task<List<UnresolvedTATDetailDto>> GetDataBeyondTATForUnResolved(string region)
        {
            var now = DateTime.UtcNow;
            try
            {
                var queryData = await (from finacle in _context.finacleDatas.AsNoTracking()
                                       join remark in _context.discrepancyRemarks.AsNoTracking()
                                       on finacle.Id equals remark.AppId
                                       where finacle.REGION == region &&
                                             finacle.ISRESOLVED == false &&
                                             finacle.DEADLINE < now
                                       select new { finacle, remark })
                                       .ToListAsync();

                return queryData.Select(x => MapToSingleDto(x.finacle, x.remark)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Joined Beyond TAT Unresolved for {Region}", region);
                return new List<UnresolvedTATDetailDto>();
            }
        }
        private UnresolvedTATDetailDto MapToSingleDto(FinacleData finacle, DiscrepancyRemark remark)
        {
            var dto = _mapper.Map<UnresolvedTATDetailDto>(finacle);

            // Manually map fields from the remark table
            dto.Discrepancies = remark.Discrepancies;
            dto.Remarks = remark.Remarks;
            dto.RecomendedAction = remark.RecomendedAction;
            dto.Responsibility = remark.Responsibility;
            dto.Deadline = finacle.DEADLINE;

            return dto;
        }
    }
}