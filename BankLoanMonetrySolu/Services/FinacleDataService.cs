using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Publishing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankLoanMonetrySolu.Services
{
    public class FinacleDataService : IFinacleData
    {
        private readonly DALContext _context;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FinacleDataService> _logger;

        public FinacleDataService(
            DALContext context,
            IMapper mapper,
            IEventPublisher eventPublisher,
            IHttpContextAccessor httpContextAccessor,
            ILogger<FinacleDataService> logger)
        {
            _context = context;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<FinacleResponseDto> GetAllAprovedFinacleData(string region)
        {
            try
            {
                _logger.LogInformation("Fetching approved Finacle records for Region: {Region}", region);
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == true)
                    .ToListAsync();

                var dto = _mapper.Map<List<FinacleDataDto>>(data);

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} approved records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching approved data for region: {Region}", region);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetAllConfirmedFinacleData(string region)
        {
            try
            {
                _logger.LogInformation("Fetching confirmed Finacle records for Region: {Region}", region);
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == false)
                    .ToListAsync();

                var dto = _mapper.Map<List<FinacleDataDto>>(data);
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} confirmed records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching confirmed data for region: {Region}", region);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetALlFincaleData()
        {
            try
            {
                _logger.LogInformation("Fetching all Finacle data with Customer details.");
                var entities = await _context.finacleDatas
                    .Include(f => f.Customer)
                    .AsNoTracking()
                    .ToListAsync();

                var dtoList = _mapper.Map<List<FinacleDataDto>>(entities);
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{dtoList.Count} Finacle records retrieved",
                    DataList = dtoList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Global Finacle data fetch failed.");
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetALLLoans()
        {
            try
            {
                _logger.LogInformation("Retrieving all customer loan details.");
                var entities = await _context.customerDetails.AsNoTracking().ToListAsync();
                var dtoList = _mapper.Map<List<CustomerDetailsDto>>(entities);

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{dtoList.Count} loan records retrieved",
                    detailsDtos = dtoList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer loan details.");
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetAllReConfirmedFinacleDataByRegion(string region)
        {
            try
            {
                _logger.LogInformation("Fetching re-confirmed records for Region: {Region}", region);
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == true && f.APPROVED == false && f.ISRESOLVED == false)
                    .ToListAsync();

                var dto = _mapper.Map<List<FinacleDataDto>>(data);
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} re-confirmed records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching re-confirmed data for region: {Region}", region);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetAllPendingConfirmationByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
            {
                _logger.LogWarning("Pending confirmation fetch failed: Region parameter is null.");
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = "Region is required" };
            }

            try
            {
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == false && f.APPROVED == false)
                    .ToListAsync();

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} pending records found for {region}",
                    DataList = _mapper.Map<List<FinacleDataDto>>(data)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending confirmations for region: {Region}", region);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FinacleResponseDto> GetResolvedFinacleData(string region)
        {
            try
            {
                _logger.LogInformation("Fetching resolved Finacle records for Region: {Region}", region);
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == true && f.APPROVED == false && f.ISRESOLVED == true)
                    .ToListAsync();

                var dto = _mapper.Map<List<FinacleDataDto>>(data);
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} Resolved records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching resolved data for region: {Region}", region);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        // --- Stats Methods ---
        public async Task<int> GetTotalPendingCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == false && f.APPROVED == false && f.RECONFIRMED == false && f.ISRESOLVED == false);

        public async Task<int> GetTotalConfirmedCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == false && f.ISRESOLVED == false);

        public async Task<int> GetTotalReConfirmedCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == true && f.APPROVED == false && f.ISRESOLVED == false);

        public async Task<int> GetTotalApproveCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == true && f.ISRESOLVED == false);

        public async Task<int> GetTotalResolvedCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == true && f.APPROVED == false && f.ISRESOLVED == true);

        public async Task<bool> UpdateFinacleStatus(FinacleStatusUpdateDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            try
            {
                if (!Guid.TryParse(dto.ApplicationId, out var appGuid)) throw new ArgumentException("Invalid Application ID");

                var record = await _context.finacleDatas.FirstOrDefaultAsync(x => x.Id == appGuid);
                if (record == null) return false;

                if (!Enum.TryParse<AuditAction>(dto.Action, true, out var actionEnum))
                {
                    throw new ArgumentException($"Invalid action: {dto.Action}");
                }

                // 1. Logic for Audit Trail
                _context.audits.Add(new Audit
                {
                    Id = Guid.NewGuid(),
                    AppId = appGuid,
                    UserId = userId,
                    IpAddress = ipAddress,
                    CreatedAt = DateTime.UtcNow,
                    Action = actionEnum
                });

                // 2. Update record status and Dynamic Deadline
                var actionTime = DateTime.UtcNow;
                var actionLower = dto.Action.ToLower();

                switch (actionLower)
                {
                    case "confirm":
                        record.CONFIRMED = true;
                        break;
                    case "approve":
                        record.APPROVED = true;
                        break;
                    case "reject":
                        record.RECONFIRMED = true;
                        // Assign the deadline from the DTO if provided
                        if (dto.Deadline.HasValue)
                        {
                            record.DEADLINE = dto.Deadline.Value;
                        }
                        else
                        {
                            _logger.LogWarning("Reject action received without a Deadline for AppId: {AppId}", dto.ApplicationId);
                        }
                        break;
                    case "resolve":
                        record.ISRESOLVED = true;
                        break;
                }

                record.PROCESSEDATE = actionTime;

                await _context.SaveChangesAsync();

                // 3. TRIGGER THE EVENTS
                try
                {
                    switch (actionLower)
                    {
                        case "confirm": await _eventPublisher.PublishAsync(new ConfirmedEvent(appGuid)); break;
                        case "approve": await _eventPublisher.PublishAsync(new ApprovedEvents(appGuid)); break;
                        case "reject": await _eventPublisher.PublishAsync(new RejectedEvent(appGuid)); break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Event publication failed for AppId: {AppId}", dto.ApplicationId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure updating status for AppId: {AppId}", dto.ApplicationId);
                return false;
            }
        }

        public async Task<FinacleResponseDto> GetCustormerAndLoanDetailsByAppId(string appId)
        {
            _logger.LogInformation("Fetching full details for AppId: {AppId}", appId);
            try
            {
                if (!Guid.TryParse(appId, out Guid appGuid)) return new FinacleResponseDto { IsSuccess = false, ResponseMessage = "Invalid ID" };

                var customer = await _context.finacleDatas.Include(c => c.Customer).FirstOrDefaultAsync(c => c.Id == appGuid);
                if (customer == null) return new FinacleResponseDto { IsSuccess = false, ResponseMessage = "Customer not found" };

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "Success",
                    Data = _mapper.Map<FinacleDataDto>(customer)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching loan details for AppId: {AppId}", appId);
                return new FinacleResponseDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<FincaleCompareDataDto> GetFinacleCompareData(string acctNum)
        {
            _logger.LogInformation("Comparing Finacle data for Account: {AcctNum}", acctNum);
            try
            {
                var data = await _context.finacleDatas
                    .Where(c => c.ACCOUNT_NUMBER == acctNum)
                    .FirstOrDefaultAsync();

                if (data == null) return new FincaleCompareDataDto { IsSuccess = false, ResponseMessage = "Record not found" };

                var result = _mapper.Map<FincaleCompareDataDto>(data);
                result.TOTAL_BALANCE = data.TOTAL_BALANCE_FCY != 0 ? data.TOTAL_BALANCE_FCY : data.TOTAL_BALANCE_LCY;
                result.INT_RATE_DISPLAY = data.INT_RATE.HasValue ? $"{data.INT_RATE.Value}%" : "0%";
                result.FX_RATE_DISPLAY = $"{data.FX_RATE}%";
                result.IsSuccess = true;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Comparison error for Account: {AcctNum}", acctNum);
                return new FincaleCompareDataDto { IsSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<ServiceResponseDto> UpsertDiscrepancyRemark(DiscrepancyRemarkDto dto)
        {
            try
            {
                // 1. Validation
                if (dto.AppId == Guid.Empty)
                    return new ServiceResponseDto { IsSuccess = false, ResponseMessage = "AppId is required." };

                DiscrepancyRemark record = null;

                // 2. Lookup - ONLY by ID
                // If the ID is empty, we don't even try to find a record.
                if (dto.Id != Guid.Empty)
                {
                    record = await _context.discrepancyRemarks.FindAsync(dto.Id);
                }

                if (record != null)
                {
                    // --- UPDATE CASE ---
                    _logger.LogInformation("Updating SPECIFIC row: {Id}", dto.Id);
                    record.Remarks = dto.Remarks;
                    record.RecomendedAction = dto.RecomendedAction;
                    record.Responsibility = dto.Responsibility;
                    record.Discrepancies = dto.Discrepancies;

                    _context.discrepancyRemarks.Update(record);
                }
                else
                {
                    // --- INSERT CASE ---
                    // If the code reaches here, it means:
                    // a) dto.Id was Empty
                    // b) OR dto.Id was provided but not found in DB
                    _logger.LogInformation("Inserting NEW row for AppId: {AppId}", dto.AppId);

                    var newRemark = new DiscrepancyRemark
                    {
                        Id = Guid.NewGuid(), // ALWAYS generate a fresh ID here
                        AppId = dto.AppId,
                        Discrepancies = dto.Discrepancies,
                        Remarks = dto.Remarks,
                        RecomendedAction = dto.RecomendedAction,
                        Responsibility = dto.Responsibility,
                        CreatedAt = DateTime.Now
                    };

                    await _context.discrepancyRemarks.AddAsync(newRemark);
                }

                await _context.SaveChangesAsync();
                return new ServiceResponseDto { IsSuccess = true, ResponseMessage = "Success" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upsert failed");
                // Check for InnerException to see if the DB is still complaining about a constraint
                var msg = ex.InnerException?.Message ?? ex.Message;
                return new ServiceResponseDto { IsSuccess = false, ResponseMessage = "Error: " + msg };
            }
        }

        public async Task<List<DiscrepancyRemarkDto>> GetRemarkByAppId(Guid appId)
        {
            try
            {
                // 1. Filter the table for all records matching the AppId
                var remarks = await _context.discrepancyRemarks
                    .AsNoTracking()
                    .Where(x => x.AppId == appId)
                    .ToListAsync();

                // 2. Map the list of entities to a list of DTOs
                return remarks.Select(remark => new DiscrepancyRemarkDto
                {
                    Id = remark.Id,
                    AppId = remark.AppId,
                    Discrepancies = remark.Discrepancies,
                    Remarks = remark.Remarks,
                    RecomendedAction = remark.RecomendedAction,
                    Responsibility = remark.Responsibility
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving remarks list for AppId: {AppId}", appId);
                return new List<DiscrepancyRemarkDto>(); // Return empty list on error
            }
        }
        public async Task<Guid> SaveFilesAsync(UploadFilesDto dto)
        {
            _logger.LogInformation("Starting file upload for AppId: {AppId}", dto.AppId);
            using var transaction = await _context.Database.BeginTransactionAsync();
            var savedFilePaths = new List<string>();

            try
            {
                var finacleData = await _context.finacleDatas
                    .Include(x => x.LoanFileBatch)
                    .ThenInclude(b => b.Files)
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(dto.AppId));

                if (finacleData == null) throw new Exception("Application not found");

                var batch = finacleData.LoanFileBatch ?? new LoanFileBatch { Id = Guid.NewGuid(), AppId = dto.AppId };
                if (finacleData.LoanFileBatch == null)
                {
                    _context.LoanFileBatch.Add(batch);
                    finacleData.LoanFileBatch = batch;
                }

                foreach (var item in dto.FileItems)
                {
                    var extension = Path.GetExtension(item.File.FileName).Replace(".", "");
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", item.DocType.ToString());
                    Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, $"{batch.Id}.{extension}");

                    // Overwrite existing physical file
                    if (File.Exists(filePath)) File.Delete(filePath);

                    // Clean existing record for this DocType in this batch
                    var existingRecord = batch.Files.FirstOrDefault(f => f.DocumentType == item.DocType);
                    if (existingRecord != null) _context.LoanFileItem.Remove(existingRecord);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.File.CopyToAsync(stream);
                    }
                    savedFilePaths.Add(filePath);

                    batch.Files.Add(new LoanFileItem
                    {
                        Id = Guid.NewGuid(),
                        FileBatchId = batch.Id,
                        FileName = Path.GetFileNameWithoutExtension(item.File.FileName),
                        FileFormat = extension,
                        DocumentType = item.DocType
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Files successfully saved and batch {BatchId} committed.", batch.Id);
                return batch.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "File save failed. Rolling back physical files...");
                foreach (var path in savedFilePaths) if (File.Exists(path)) File.Delete(path);
                throw;
            }
        }

        public async Task<List<FileViewDto>> GetAllFilesByBatchIdAsync(Guid batchId)
        {
            _logger.LogInformation("Retrieving file list for BatchId: {BatchId}", batchId);
            var files = await _context.LoanFileItem.Where(f => f.FileBatchId == batchId).ToListAsync();
            return files.Select(f => new FileViewDto
            {
                AppId = f.Id,
                FileName = f.FileName,
                DocType = f.DocumentType.ToString(),
                ContentType = GetMimeType(f.FileFormat),
            }).ToList();
        }

        private string GetMimeType(string extension)
        {
            extension = extension?.TrimStart('.').ToLower();
            return extension switch
            {
                "pdf" => "application/pdf",
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }


        public async Task<string> AddFile(FileViewDto file)
        {
            if (file.AppId == null || string.IsNullOrEmpty(file.FileName))
                return "Invalid file data.";

            var newFile = new LoanFileItem
            {
                Id = Guid.NewGuid(),
                AppId = file.AppId.Value, // direct relationship
                FileName = file.FileName,
                FileFormat = file.ContentType ?? "unknown",
                IsFileVerified = file.IsVeriFied,
                DocumentType = Enum.TryParse<LoanDocumentType>(file.DocType, true, out var matchedType)
                                ? matchedType
                                : LoanDocumentType.Other,
                CustomDocumentType = Enum.TryParse<LoanDocumentType>(file.DocType, true, out _)
                                    ? null
                                    : file.DocType
            };

            _context.LoanFileItem.Add(newFile);
            await _context.SaveChangesAsync();

            return newFile.Id.ToString();
        }

        public async Task<List<FileEditDto>> GetFileByAppIdAsync(Guid appId)
        {
            return await _context.LoanFileItem
                .Where(x => x.AppId == appId)
                .Select(f => new FileEditDto
                {
                    Id = f.Id,
                    AppId = f.AppId,
                    FileName = f.FileName,
                    // If it's 'Other', show the custom string; otherwise show the Enum name
                    DocType = f.DocumentType == LoanDocumentType.Other
                              ? f.CustomDocumentType
                              : f.DocumentType.ToString(),
                    ContentType = f.FileFormat,
                    IsVeriFied = f.IsFileVerified
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateFileStatus(Guid fileId, bool verify)
        {
            var file = await _context.LoanFileItem.FindAsync(fileId);
            if (file == null) return false;

            file.IsFileVerified = verify;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<FileEditDto>> EditFileByAppIdAsync(Guid appId, FileEditDto updateDto)
        {
            // 1. Find the specific file by its unique ID
            var existingFile = await _context.LoanFileItem
                .FirstOrDefaultAsync(x => x.Id == updateDto.Id && x.AppId == appId);

            if (existingFile == null)
            {
                throw new Exception("File record not found.");
            }

            // 2. Update properties
            existingFile.FileName = updateDto.FileName ?? existingFile.FileName;
            existingFile.FileFormat = updateDto.ContentType ?? existingFile.FileFormat;
            existingFile.IsFileVerified = updateDto.IsVeriFied;

            // 3. Handle the Enum/Custom Type Logic
            if (Enum.TryParse<LoanDocumentType>(updateDto.DocType, true, out var matchedType))
            {
                existingFile.DocumentType = matchedType;
                existingFile.CustomDocumentType = matchedType == LoanDocumentType.Other
                                                  ? updateDto.DocType : null;
            }
            else
            {
                existingFile.DocumentType = LoanDocumentType.Other;
                existingFile.CustomDocumentType = updateDto.DocType;
            }

            // 4. Save changes
            await _context.SaveChangesAsync();

            // 5. Return the refreshed list for the AppId
            return await GetFileByAppIdAsync(appId);
        }
        public async Task<string> DeleteFileByFileId(Guid appId, Guid fileId)
        {
            // 1. Find the specific file by its unique ID and ensure it belongs to the AppId
            var existingFile = await _context.LoanFileItem
                .FirstOrDefaultAsync(x => x.Id == fileId && x.AppId == appId);

            if (existingFile == null)
            {
                return "File record not found.";
            }

            // 2. Remove from the specific DbSet
            _context.LoanFileItem.Remove(existingFile);

            // 3. Save changes
            await _context.SaveChangesAsync();

            return "The file was deleted successfully.";
        }
    }
}