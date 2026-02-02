using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankLoanMonetrySolu.Services
{
    public class ConventTracker : IConventTracker
    {
        private readonly DALContext _context;

        public ConventTracker(DALContext context)
        {
            _context = context;
        }

        public async Task<LoanCovenantTrackerResponse> GetLoanCovenantTrackerResponsesAsync(string AppId)
        {
            try
            {
                if (!Guid.TryParse(AppId, out Guid guidAppId))
                {
                    throw new ArgumentException("Invalid AppId format. Must be a valid Guid.");
                }

                var financials = await _context.financialCovenants
                    .Where(x => x.AppId == guidAppId)
                    .Select(f => new ConventFinacialDto
                    {
                        CovenantType = f.CovenantType,
                        Description = f.Description,
                        TargetValue = f.TargetValue,
                        Frequency = f.Frequency,
                        DataSource = f.DataSource
                    }).ToListAsync();

                var nonFinancials = await _context.nonFinancialCovenants
                    .Where(x => x.AppId == guidAppId)
                    .Select(nf => new NonFinacialConventDto
                    {
                        CovenantType = nf.CovenantType,
                        Description = nf.Description,
                        Frequency = nf.Frequency
                    }).ToListAsync();

                var accountNumber = financials.FirstOrDefault()?.AccountNumber
                                    ?? nonFinancials.FirstOrDefault()?.AccountNumber
                                    ?? "N/A";

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
                throw new Exception($"Error retrieving covenant tracker: {ex.Message}");
            }
        }

        public async Task<string> EditNonFinaciaConeventTracker(string AppId, EditNonFinancialCovenantDto updateDto)
        {
            try
            {
                if (!Guid.TryParse(AppId, out Guid guidAppId))
                {
                    return "Invalid Guid Format";
                }

                var existingCovenant = await _context.nonFinancialCovenants
                    .FirstOrDefaultAsync(x => x.AppId == guidAppId && x.Id == updateDto.Id);

                if (existingCovenant == null)
                {
                    return "Covenant record not found.";
                }

                // Update CovenantType (e.g., Negative Pledge)
                if (!string.IsNullOrWhiteSpace(updateDto.CovenantType))
                {
                    existingCovenant.CovenantType = updateDto.CovenantType;
                }

                // Update Description (e.g., Reporting requirements)
                if (!string.IsNullOrWhiteSpace(updateDto.Description))
                {
                    existingCovenant.Description = updateDto.Description;
                }

                // Update Frequency
                if (updateDto.Frequency.HasValue)
                {
                    existingCovenant.Frequency = updateDto.Frequency.Value;
                }

                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                return $"Update Failed: {ex.Message}";
            }
        }
    }
}