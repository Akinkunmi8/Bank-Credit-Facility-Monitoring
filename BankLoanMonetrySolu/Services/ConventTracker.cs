using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
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

        public async Task<FinacialCovenantTrackerResponse> AddFinacialConvenatTracker(string AppId, ConventFinacialDto updateDto)
        {
            // 1. Validate GUID format
            if (!Guid.TryParse(AppId, out Guid guidAppId))
            {
                return new FinacialCovenantTrackerResponse { Message = "Invalid appId"};
            }

            // 2. Fetch account record (Fixed the '=' vs '==' bug here)
            var accountRecord = await _context.finacleDatas
                .FirstOrDefaultAsync(f => f.Id == guidAppId);

            if (accountRecord == null)
            {
                return new FinacialCovenantTrackerResponse { Message = "No account record found for the provided ID" };
            }
            var newData = new FinancialCovenant
            {
                AppId = guidAppId,
                CovenantType = updateDto.CovenantType,
                Description = updateDto.Description,
                TargetValue = updateDto.TargetValue ?? 0,
                Frequency = updateDto.Frequency,
                DataSource = updateDto.DataSource,
                AccountNumber = accountRecord.ACCOUNT_NUMBER

            };
            try
            {
                await _context.financialCovenants.AddAsync(newData);
                await _context.SaveChangesAsync();
                return new FinacialCovenantTrackerResponse
                {
                    AppId = AppId,
                    isSuccess = true,
                    Message = "The New Financial Covenant added",
                    // Manually convert Model to DTO here:
                    Financials = new ConventFinacialDto
                    {
                        CovenantType = newData.CovenantType,
                        Description = newData.Description,
                        TargetValue = newData.TargetValue,
                        Frequency = newData.Frequency,
                        DataSource = newData.DataSource
                    }
                };
            }
            catch(Exception ex)
            {
                // Log the exception (ex) here if you have a logger
                return new FinacialCovenantTrackerResponse
                {
                    isSuccess = false,
                    Message = $"Error saving data: {ex.Message}"
                };
            }
        }

        public async Task<BulkFinacialCovenantTrackerResponse> UploadFinancialCovenantExcel(IFormFile file)
        {
            var errorRows = new List<string>();

            if (file == null || file.Length == 0)
                return new BulkFinacialCovenantTrackerResponse { Message = "No file uploaded", isSuccess = false, Errors = errorRows };

            var newCovenants = new List<FinancialCovenant>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null || worksheet.Dimension == null)
                            return new BulkFinacialCovenantTrackerResponse { Message = "Excel sheet is empty", isSuccess = false };

                        int rowCount = worksheet.Dimension.Rows;
                        var excelEntries = new List<TempExcelRow>();

                        // 1. Extraction & Initial Validation
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var appIdRaw = worksheet.Cells[row, 1].Value?.ToString();

                            if (Guid.TryParse(appIdRaw, out Guid guidId))
                            {
                                excelEntries.Add(new TempExcelRow
                                {
                                    RowIndex = row,
                                    AppId = guidId,
                                    Type = worksheet.Cells[row, 2].Value?.ToString(),
                                    Desc = worksheet.Cells[row, 3].Value?.ToString(),
                                    Value = decimal.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out var v) ? v : 0,
                                    Freq = worksheet.Cells[row, 5].Value?.ToString(),
                                    Source = worksheet.Cells[row, 6].Value?.ToString()
                                });
                            }
                            else if (!string.IsNullOrWhiteSpace(appIdRaw))
                            {
                                errorRows.Add($"Row {row}: Invalid GUID format ({appIdRaw})");
                            }
                        }

                        // 2. Batch Fetch Account Numbers
                        var uniqueAppIds = excelEntries.Select(x => x.AppId).Distinct().ToList();
                        var accountMap = await _context.finacleDatas
                            .Where(f => uniqueAppIds.Contains(f.Id))
                            .ToDictionaryAsync(f => f.Id, f => f.ACCOUNT_NUMBER);

                        // 3. Mapping to Domain Model
                        foreach (var entry in excelEntries)
                        {
                            if (accountMap.TryGetValue(entry.AppId, out var accNumber))
                            {
                                newCovenants.Add(new FinancialCovenant
                                {
                                    AppId = entry.AppId,
                                    CovenantType = entry.Type,
                                    Description = entry.Desc,
                                    TargetValue = entry.Value,
                                    Frequency = Enum.TryParse<FrequencyEnum>(entry.Freq, true, out var freq) ? freq : FrequencyEnum.Annually,
                                    DataSource = entry.Source,
                                    AccountNumber = accNumber
                                });
                            }
                            else
                            {
                                errorRows.Add($"Row {entry.RowIndex}: AppId {entry.AppId} not found in database.");
                            }
                        }
                    }
                }

                // 4. Persistence
                if (newCovenants.Any())
                {
                    await _context.financialCovenants.AddRangeAsync(newCovenants);
                    await _context.SaveChangesAsync();

                    return new BulkFinacialCovenantTrackerResponse
                    {
                        isSuccess = true,
                        Message = $"Successfully uploaded {newCovenants.Count} records.",
                        // Map the Model list to the DTO list
                        uploadedDatas = newCovenants.Select(x => new ConventFinacialDto
                        {
                            CovenantType = x.CovenantType,
                            Description = x.Description,
                            TargetValue = x.TargetValue,
                            Frequency = x.Frequency,
                            DataSource = x.DataSource
 
                            // Map any other properties your DTO needs
                        }).ToList(),
                        Errors = errorRows
                    };
                }
            }
            catch (Exception ex)
            {
                return new BulkFinacialCovenantTrackerResponse { isSuccess = false, Message = $"Critical Error: {ex.Message}" };
            }

            return new BulkFinacialCovenantTrackerResponse { isSuccess = false, Message = "No records were saved.", Errors = errorRows };
        }

        // Helper class for parsing
        private class TempExcelRow
        {
            public int RowIndex { get; set; }
            public Guid AppId { get; set; }
            public string Type { get; set; }
            public string Desc { get; set; }
            public decimal Value { get; set; }
            public string Freq { get; set; }
            public string Source { get; set; }
        }
    }
}