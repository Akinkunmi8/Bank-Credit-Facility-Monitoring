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
    public class ConventTracker : IConventTracker
    {
        private readonly DALContext _context;
        private readonly ILogger<ConventTracker> _logger;

        public ConventTracker(DALContext context, ILogger<ConventTracker> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<LoanCovenantTrackerResponse> GetLoanCovenantTrackerResponsesAsync(string AppId)
        {
            _logger.LogInformation("Retrieving loan covenants for AppId: {AppId}", AppId);

            try
            {
                if (!Guid.TryParse(AppId, out Guid guidAppId))
                {
                    _logger.LogWarning("Invalid AppId format received: {AppId}", AppId);
                    throw new ArgumentException("Invalid AppId format. Must be a valid Guid.");
                }

                // Fetch Financial Covenants
                var financials = await _context.financialCovenants
                    .AsNoTracking()
                    .Where(x => x.AppId == guidAppId)
                    .Select(f => new ConventFinacialDto
                    {
                        CovenantType = f.CovenantType,
                        Description = f.Description,
                        TargetValue = f.TargetValue,
                        Frequency = f.Frequency,
                        DataSource = f.DataSource,
                        AccountNumber = f.AccountNumber // Assuming this property exists in your model
                    }).ToListAsync();

                // Fetch Non-Financial Covenants
                var nonFinancials = await _context.nonFinancialCovenants
                    .AsNoTracking()
                    .Where(x => x.AppId == guidAppId)
                    .Select(nf => new NonFinacialConventDto
                    {
                        CovenantType = nf.CovenantType,
                        Description = nf.Description,
                        Frequency = nf.Frequency,
                        AccountNumber = nf.AccountNumber // Assuming this property exists in your model
                    }).ToListAsync();

                var accountNumber = financials.FirstOrDefault()?.AccountNumber
                                    ?? nonFinancials.FirstOrDefault()?.AccountNumber
                                    ?? "N/A";

                _logger.LogInformation("Successfully retrieved covenants for Account: {AccountNumber}", accountNumber);

                return new LoanCovenantTrackerResponse
                {
                    AppId = AppId,
                    AccountNumber = accountNumber,
                    Financials = financials,
                    NonFinancials = nonFinancials
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve covenants for AppId: {AppId}", AppId);
                throw; // Re-throw to allow global error handler to capture it
            }
        }

        public async Task<string> EditNonFinaciaConeventTracker(string AppId, EditNonFinancialCovenantDto updateDto)
        {
            _logger.LogInformation("Attempting to update Non-Financial Covenant ID for AppId: {AppId}",  AppId);

            try
            {
                if (!Guid.TryParse(AppId, out Guid guidAppId))
                {
                    return "Invalid Guid Format";
                }

                var existingCovenant = await _context.nonFinancialCovenants
                    .FirstOrDefaultAsync(x => x.AppId == guidAppId);

                if (existingCovenant == null)
                {
                    _logger.LogWarning("Covenant update failed: Record not found for Id ");
                    return "Covenant record not found.";
                }

                // Update Fields
                if (!string.IsNullOrWhiteSpace(updateDto.CovenantType))
                    existingCovenant.CovenantType = updateDto.CovenantType;

                if (!string.IsNullOrWhiteSpace(updateDto.Description))
                    existingCovenant.Description = updateDto.Description;

                if (updateDto.Frequency.HasValue)
                    existingCovenant.Frequency = updateDto.Frequency.Value;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated Non-Financial Covenant");
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error during covenant update for AppId: {AppId}", AppId);
                return $"Update Failed: {ex.Message}";
            }
        }

        public async Task<string> AddNonFinaciaConeventTracker(string AppId, AddNonFincialConvDto addDto)
        {
            // 1. Validate GUID format
            if (!Guid.TryParse(AppId, out Guid guidAppId))
            {
                return "Invalid Guid Format";
            }

            // 2. Fetch account record (Fixed the '=' vs '==' bug here)
            var accountRecord = await _context.finacleDatas
                .FirstOrDefaultAsync(f => f.Id == guidAppId);

            if (accountRecord == null)
            {
                return "No account record found for the provided ID";
            }

            // 3. Map and create the new record
            var newData = new NonFinancialCovenant
            {
                AppId = guidAppId,
                AccountNumber = accountRecord.ACCOUNT_NUMBER?.ToString() ?? "N/A",
                CovenantType = addDto.CovenantType,
                Description = addDto.Description,
                Frequency = addDto.Frequency ?? FrequencyEnum.Monthly,
                
            };

            try
            {
                await _context.nonFinancialCovenants.AddAsync(newData);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if you have a logger
                return $"Error saving data: {ex.Message}";
            }
        }
    }
}