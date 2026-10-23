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
                .Where(f =>
                    f.REGION == region &&
                    f.CONFIRMED == false &&
                    f.RECONFIRMED == true &&
                    f.APPROVED == false &&
                    f.ISRESOLVED == false &&
                    f.RESOLUTIONSUBMITTED == false)
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

        // Get the Resolution review befor submited 

        public async Task<FinacleResponseDto> GetCMOReviewByRegion(string region)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching CMO resolution review records for Region: {Region}",
                    region);

                var data = await _context.finacleDatas
                    .AsNoTracking()
                    .Where(f =>
                        f.REGION == region &&
                        f.RECONFIRMED == true&&
                        !f.APPROVED == false&&
                        !f.ISRESOLVED  == false&&
                        f.RESOLUTIONSUBMITTED == true)
                    .ToListAsync();

                var dto = _mapper.Map<List<FinacleDataDto>>(data);

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage =
                        $"{data.Count} CMO resolution review record(s) retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching CMO resolution review data for Region: {Region}",
                    region);

                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = ex.Message
                };
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
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == false );

        public async Task<int> GetTotalReConfirmedCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == false && f.RECONFIRMED == true && f.APPROVED == false && f.ISRESOLVED == false);

        public async Task<int> GetTotalApproveCount(string region) =>
            await _context.finacleDatas.AsNoTracking().CountAsync(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == true && f.ISRESOLVED == false && f.RESOLUTIONSUBMITTED == false);

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
                        record.ISRESOLVED = false;

                        // New resolution cycle
                        record.RESOLUTIONSUBMITTED = false;
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
                        record.RECONFIRMED = true;
                        break;

                    case "return-resolution":

                        record.RECONFIRMED = true;
                        record.RESOLUTIONSUBMITTED = false;
                        record.ISRESOLVED = false;

                        if (dto.Deadline.HasValue)
                        {
                            record.DEADLINE = dto.Deadline.Value;
                        }
                        else
                        {
                            _logger.LogWarning(
                                "CMO returned application {AppId} without a resolution deadline.",
                                appGuid);
                        }

                        break;


                    case "accept-resolution":

                        record.RECONFIRMED = false;
                        record.CONFIRMED = true;
                        record.RESOLUTIONSUBMITTED = false;
                        record.ISRESOLVED = true;
                        record.APPROVED = false;
                        record.DEADLINE = null;

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
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "AppId is required."
                    };
                }

                if (string.IsNullOrWhiteSpace(dto.Discrepancies))
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "Discrepancy is required."
                    };
                }

                DiscrepancyRemark? record = null;

                // 2. First choice: find exact record by Id
                if (dto.Id != Guid.Empty)
                {
                    record = await _context.discrepancyRemarks
                        .FirstOrDefaultAsync(x =>
                            x.Id == dto.Id &&
                            x.AppId == dto.AppId);
                }

                // 3. Fallback:
                // First CMO submission may not have an Id yet.
                // Find the same discrepancy belonging to the same application.
                if (record == null)
                {
                    record = await _context.discrepancyRemarks
                        .FirstOrDefaultAsync(x =>
                            x.AppId == dto.AppId &&
                            x.Discrepancies == dto.Discrepancies);
                }

                if (record != null)
                {
                    // UPDATE
                    _logger.LogInformation(
                        "Updating discrepancy remark {Id} for AppId {AppId}",
                        record.Id,
                        dto.AppId);

                    record.Remarks = dto.Remarks;
                    record.RecomendedAction = dto.RecomendedAction;
                    record.Responsibility = dto.Responsibility;
                    record.Discrepancies = dto.Discrepancies;
                }
                else
                {
                    // INSERT
                    var newRemark = new DiscrepancyRemark
                    {
                        Id = Guid.NewGuid(),
                        AppId = dto.AppId,
                        Discrepancies = dto.Discrepancies,
                        Remarks = dto.Remarks,
                        RecomendedAction = dto.RecomendedAction,
                        Responsibility = dto.Responsibility,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.discrepancyRemarks.AddAsync(newRemark);

                    _logger.LogInformation(
                        "Creating discrepancy remark {Id} for AppId {AppId}: {Discrepancy}",
                        newRemark.Id,
                        dto.AppId,
                        dto.Discrepancies);
                }

                await _context.SaveChangesAsync();

                return new ServiceResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "Discrepancy remark saved successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to save discrepancy remark for AppId {AppId}",
                    dto.AppId);

                var message = ex.InnerException?.Message ?? ex.Message;

                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Error: " + message
                };
            }
        }
        

        public async Task<ServiceResponseDto> ResolveDiscrepancy(ResolveDiscrepancyDto dto)
        {
            var userName =
                _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.Name)?.Value
                ??
                _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.Email)?.Value
                ?? "Unknown";

            var record = await _context.discrepancyRemarks
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.AppId == dto.AppId);

            if (record == null)
            {
                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Discrepancy record not found."
                };
            }

            record.ResolutionAction = dto.ResolutionAction;
            record.ResolutionRemarks = dto.ResolutionRemarks;
            record.RespondedBy = userName;
            record.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ServiceResponseDto
            {
                IsSuccess = true,
                ResponseMessage = "Resolution submitted successfully."
            };
        }

        public async Task<ServiceResponseDto> SubmitDiscrepancyResponseAsync(SubmitDiscrepancyResponseDto dto)
        {
            try
            {
                if (dto.Id == Guid.Empty || dto.AppId == Guid.Empty)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "Invalid discrepancy information."
                    };
                }

                if (string.IsNullOrWhiteSpace(dto.ResolutionAction))
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "Resolution action is required."
                    };
                }

                var record = await _context.discrepancyRemarks
                    .FirstOrDefaultAsync(x =>
                        x.Id == dto.Id &&
                        x.AppId == dto.AppId);

                if (record == null)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "Discrepancy record was not found."
                    };
                }

                var httpContext = _httpContextAccessor.HttpContext;

                var respondedBy =
                    httpContext?.User.FindFirst(ClaimTypes.Name)?.Value
                    ?? httpContext?.User.FindFirst(ClaimTypes.Email)?.Value
                    ?? httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? "Unknown";

                record.ResolutionAction = dto.ResolutionAction.Trim();
                record.ResolutionRemarks = dto.ResolutionRemarks?.Trim();
                record.RespondedBy = respondedBy;
                record.RespondedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new ServiceResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "Response saved successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to save RM response for discrepancy {Id}",
                    dto.Id);

                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Unable to save discrepancy response."
                };
            }
        }

        public async Task<ServiceResponseDto> SubmitResolutionForReviewAsync(Guid appId)
        {
            try
            {
                var loan = await _context.finacleDatas
                    .FirstOrDefaultAsync(x => x.Id == appId);

                if (loan == null)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "Loan application not found."
                    };
                }

                // It must actually have been returned by CMO
                if (!loan.RECONFIRMED || loan.ISRESOLVED)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage =
                            "This application is not awaiting RM resolution."
                    };
                }

                var discrepancies = await _context.discrepancyRemarks
                    .Where(x => x.AppId == appId)
                    .ToListAsync();

                if (discrepancies.Count == 0)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage =
                            "No discrepancy records were found."
                    };
                }

                var unanswered = discrepancies
                    .Where(x =>
                        string.IsNullOrWhiteSpace(x.ResolutionAction))
                    .ToList();

                if (unanswered.Count > 0)
                {
                    return new ServiceResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage =
                            $"{unanswered.Count} discrepancy response(s) are still outstanding."
                    };
                }

                /*
                 * IMPORTANT:
                 *
                 * DO NOT SET:
                 * loan.ISRESOLVED = true;
                 *
                 * RM has only submitted responses.
                 */
                loan.RESOLUTIONSUBMITTED = true;
                loan.PROCESSEDATE = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new ServiceResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage =
                        "Resolution response submitted for CMO re-review."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed submitting resolution for CMO review. AppId {AppId}",
                    appId);

                return new ServiceResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage =
                        "Unable to submit the resolution for review."
                };
            }
        }
        public async Task<List<DiscrepancyRemarkDto>> GetRemarkByAppId(Guid appId)
        {
            try
            {
                var remarks = await _context.discrepancyRemarks
                    .AsNoTracking()
                    .Where(x => x.AppId == appId)
                    .ToListAsync();

                return remarks.Select(remark => new DiscrepancyRemarkDto
                {
                    Id = remark.Id,
                    AppId = remark.AppId,
                    Discrepancies = remark.Discrepancies,

                    // CMO instruction
                    Remarks = remark.Remarks,
                    RecomendedAction = remark.RecomendedAction,
                    Responsibility = remark.Responsibility,

                    // AO/RM response
                    ResolutionAction = remark.ResolutionAction,
                    ResolutionRemarks = remark.ResolutionRemarks,
                    RespondedBy = remark.RespondedBy,
                    RespondedAt = remark.RespondedAt

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving remarks list for AppId: {AppId}",
                    appId);

                return new List<DiscrepancyRemarkDto>();
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

        public async Task<List<FileViewDto>> GetAllFilesByRegiondAsync(string region)
        {
            _logger.LogInformation("Retrieving file list that are not on server for Region: {region}", region);
            var files = await _context.LoanFileItem.Where(f => f.Region == region && f.IsFileExitOnServer == false).ToListAsync();
            return files.Select(f => new FileViewDto
            {
                AppId = f.AppId,
                FileName = f.FileName,
                DocType = f.DocumentType.ToString(),
                ContentType = GetMimeType(f.FileFormat),
                RequesterUsername = f.RequesterUsername,
            }).ToList();
        }

        public async Task<int> GetTotalUnsaveFileCount(string region) =>
            await _context.LoanFileItem
                .AsNoTracking()
                .CountAsync(f => f.Region == region && !f.IsFileExitOnServer);
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
            if (file.AppId == null || string.IsNullOrWhiteSpace(file.FileName))
                return "Invalid file data.";

            var appId = file.AppId.Value;

            // Confirm that the application exists
            var applicationExists = await _context.finacleDatas
                .AnyAsync(x => x.Id == appId);

            if (!applicationExists)
                return "Loan application not found.";

            // LoanFileBatch.AppId is stored as nvarchar
            var appIdString = appId.ToString();

            // Find an existing batch for this application
            var batch = await _context.LoanFileBatch
                .FirstOrDefaultAsync(x => x.AppId == appIdString);

            // Manually seeded/demo applications may not have a batch yet
            if (batch == null)
            {
                batch = new LoanFileBatch
                {
                    Id = Guid.NewGuid(),
                    AppId = appIdString,
                    CreatedAt = DateTime.UtcNow
                };

                _context.LoanFileBatch.Add(batch);
            }

            // Parse document type only once
            var isKnownDocumentType = Enum.TryParse<LoanDocumentType>(
                file.DocType,
                true,
                out var documentType);

            var newFile = new LoanFileItem
            {
                Id = Guid.NewGuid(),

                // FK -> finacleDatas.Id
                AppId = appId,

                // FK -> LoanFileBatch.Id
                FileBatchId = batch.Id,

                FileName = file.FileName,
                FileFormat = file.ContentType ?? "unknown",

                DocumentType = isKnownDocumentType
                    ? documentType
                    : LoanDocumentType.Other,

                CustomDocumentType = isKnownDocumentType
                    ? null
                    : file.DocType,

                IsFileVerified = file.IsVeriFied,
                IsFileExitOnServer = file.IsFileExitOnServer,

                RequesterUsername = file.RequesterUsername,
                Region = file.Region,

                CreatedAt = DateTime.UtcNow
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
                    IsVeriFied = f.IsFileVerified,
                    
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateFileStatus(Guid fileId, bool verify)
        {
            var file = await _context.LoanFileItem.FindAsync(fileId);

            // 1. Guard clause
            if (file == null) return false;

            // 2. Update verification status
            file.IsFileVerified = verify;

            if (verify)
            {
                file.IsFileExitOnServer = true;
            }

            // 4. Save changes
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating file status for FileId: {fileId}", fileId);
                return false;
            }
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