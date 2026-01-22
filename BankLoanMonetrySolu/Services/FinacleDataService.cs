using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using BankLoanMonetrySolu.Models;
using Contracts.Dto;
using Contracts.Publishing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class FinacleDataService : IFinacleData
    {
        private readonly DALContext _context;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FinacleDataService(DALContext context, IMapper mapper, IEventPublisher eventPublisher,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<FinacleResponseDto> GetAllAprovedFinacleData( string region)
        {
            try
            {
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == true ) // Example filter
                    .ToListAsync();
                var dto = _mapper.Map<List<FinacleDataDto>>(data); ;

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} approved records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching approved Finacle data: {ex.Message}"
                };
            }
        }

        public async Task<FinacleResponseDto> GetAllConfirmedFinacleData( string region)
        {
            try
            {
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
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching confirmed Finacle data: {ex.Message}"
                };
            }
        }

        public async Task<FinacleResponseDto> GetALlFincaleData()
        {
            try
            {
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
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching Finacle data: {ex.Message}"
                };
            }
        }

        public async Task<FinacleResponseDto> GetALLLoans()
        {
            try
            {
                var entities = await _context.customerDetails
                    .AsNoTracking()
                    .ToListAsync();

                var dtoList = _mapper.Map<List<CustomerDetailsDto>>(entities);

                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{dtoList.Count} Finacle records retrieved",
                    detailsDtos = dtoList
                };
            }
            catch (Exception ex)
            {
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching Finacle data: {ex.Message}"
                };
            }
        }



        public async Task<FinacleResponseDto> GetAllReConfirmedFinacleDataByRegion(string region)
        {
            try
            {
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == true  && f.RECONFIRMED == true && f.APPROVED == false ) // Example filter
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
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching re-confirmed Finacle data: {ex.Message}"
                };
            }
        }

        public async Task<FinacleResponseDto> GetAllPendingConfirmationByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = "Region parameter is required"
                };

            try
            {
                var data = await _context.finacleDatas
                    .Where(f => f.REGION == region && f.CONFIRMED == false  && f.RECONFIRMED == false && f.APPROVED == false)
                    .ToListAsync();
                var dto = _mapper.Map<List<FinacleDataDto>>(data); ;
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{data.Count} records retrieved for region '{region}'",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching Finacle data by region: {ex.Message}"
                };
            }
        }

        public async Task<int> GetTotalPendingCount(string region)
        {
            return await _context.finacleDatas
                .AsNoTracking()
                .CountAsync(f =>
                    f.REGION == region &&
                    f.CONFIRMED == false &&
                    f.APPROVED == false &&
                    f.RECONFIRMED == false
                );
        }


        public async Task<int> GetTotalConfirmedCount(string region)
        {
            return await _context.finacleDatas
                .AsNoTracking()
                .CountAsync(f =>
                    f.REGION == region &&
                    f.CONFIRMED == true &&
                    f.RECONFIRMED == false &&
                    f.APPROVED == false
                );
        }


        public async Task<int> GetTotalReConfirmedCount(string region)
        {
            return await _context.finacleDatas
                .AsNoTracking()
                .CountAsync(f =>
                    f.REGION == region &&
                    f.CONFIRMED == false &&
                    f.RECONFIRMED == true &&
                    f.APPROVED == false

                );
        }


        public async Task<int> GetTotalApproveCount(string region)
        {
            return await _context.finacleDatas
                .AsNoTracking()
                .CountAsync(f =>
                    f.REGION == region &&
                    f.CONFIRMED == true &&
                    f.RECONFIRMED == false &&
                    f.APPROVED == true
                );
        }

        public async Task<bool> UpdateFinacleStatus(FinacleStatusUpdateDto dto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not logged in");

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

            // Use these for audit
            var audit = new Audit
            {
                Id = Guid.NewGuid(),
                AppId = Guid.TryParse(dto.ApplicationId, out var appGuid) ? appGuid : Guid.Empty,
                UserId = userId,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow,
            };

            // Parse string action into enum
            if (!Enum.TryParse<AuditAction>(dto.Action, true, out var actionEnum))
            {
                throw new ArgumentException($"Invalid action: {dto.Action}");
            }

            audit.Action = actionEnum;

            _context.audits.Add(audit);

            // Existing status update logic
            var record = await _context.finacleDatas.FirstOrDefaultAsync(x => x.Id == audit.AppId);
            if (record == null) return false;

            var actionTime = DateTime.UtcNow;
            switch (dto.Action?.ToLower())
            {
                case "confirm":
                    record.CONFIRMED = true;
                    record.PROCESSEDATE = actionTime;
                    break;
                case "approve":
                    record.APPROVED = true;
                    record.PROCESSEDATE = actionTime;
                    break;
                case "reject":
                    record.RECONFIRMED = true;
                    record.PROCESSEDATE = actionTime;
                    break;
                default:
                    throw new ArgumentException($"Invalid action: {dto.Action}");
            }

            await _context.SaveChangesAsync();
            return true;
        }




        public async Task<FinacleResponseDto> GetCustormerAndLoanDetailsByAppId(string appId)
        {
            try
            {
                if (!Guid.TryParse(appId, out Guid appGuid))
                {
                    return new FinacleResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = $"Invalid Application ID format: {appId}"
                    };
                }

                // 1️⃣ Fetch the customer including their loan details
                var customer = await _context.finacleDatas
                    .Include(c => c.Customer)
                    .FirstOrDefaultAsync(c => c.Id == appGuid);

                if (customer == null)
                {
                    return new FinacleResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = $"No customer found for AppId {appId}"
                    };
                }

                // 2️⃣ Map to DTOs (avoids cycles)
                var customerDto = _mapper.Map<FinacleDataDto>(customer);

                // 3️⃣ Return response in your standard template
                return new FinacleResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "Customer with loan details retrieved successfully",
                    Data = customerDto
                };
            }
            catch (Exception ex)
            {
                return new FinacleResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching customer data: {ex.Message}"
                };
            }
        }

        public async Task<FincaleCompareDataDto> GetFinacleCompareData(string acctNum)
        {
            try
            {
                var data = await _context.finacleDatas
                    .Where(c => c.ACCOUNT_NUMBER == acctNum)
                    .Select(c => new FinacleDataDto
                    {
                        Id = c.Id,
                        ACCOUNT_NAME = c.ACCOUNT_NAME,
                        ACCOUNT_NUMBER = c.ACCOUNT_NUMBER,
                        SBU_NAME = c.SBU_NAME,
                        FACILITY_TYPE = c.FACILITY_TYPE,
                        TOTAL_BALANCE_FCY = c.TOTAL_BALANCE_FCY,
                        TOTAL_BALANCE_LCY = c.TOTAL_BALANCE_LCY,
                        SANCTION_LIMIT = c.SANCTION_LIMIT,
                        INT_RATE = c.INT_RATE,
                        FX_RATE = c.FX_RATE,
                        DATE_GRANTED = c.DATE_GRANTED,
                        EXPIRY_DATE = c.EXPIRY_DATE
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return new FincaleCompareDataDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "No record found for the given account number"
                    };
                }

                var result = _mapper.Map<FincaleCompareDataDto>(data);

                // FCY / LCY decision
                result.TOTAL_BALANCE = data.TOTAL_BALANCE_FCY != 0
                    ? data.TOTAL_BALANCE_FCY
                    : data.TOTAL_BALANCE_LCY;

                // 🔥 Explicit formatting
                result.INT_RATE_DISPLAY = result.INT_RATE.HasValue
                    ? $"{result.INT_RATE.Value}%"
                    : null;

                // FX_RATE is int
                result.FX_RATE_DISPLAY = $"{result.FX_RATE}%";

                result.IsSuccess = true;
                result.ResponseMessage = "Record retrieved successfully";

                return result;
            }
            catch (Exception ex)
            {
                return new FincaleCompareDataDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Failed to fetch record: {ex.Message}"
                };
            }
        }

        public async Task<Guid> SaveFilesAsync(UploadFilesDto dto)
        {
            //if (!Guid.TryParse(dto.AppId, out Guid appGuid))
            //    throw new Exception("Invalid AppId");

            // Start a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            var newlyCreatedFiles = new List<string>();

            try
            {
                var finacleData = await _context.finacleDatas
                    .Include(x => x.LoanFileBatch)
                        .ThenInclude(b => b.Files)
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(dto.AppId));

                if (finacleData == null) throw new Exception("Application not found");

                // 1. Get or Create Batch
                var batch = finacleData.LoanFileBatch;
                if (batch == null)
                {
                    batch = new LoanFileBatch { Id = Guid.NewGuid(), AppId = dto.AppId };
                    _context.LoanFileBatch.Add(batch);
                    finacleData.LoanFileBatch = batch;
                }

                foreach (var item in dto.FileItems)
                {
                    var extension = Path.GetExtension(item.File.FileName).Replace(".", "");
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", item.DocType.ToString());

                    Directory.CreateDirectory(folderPath);

                    // 2. NAME FILE AFTER BATCH ID
                    // Result: UploadedFiles\CAT\7c0bacd6-fdb9-4ba8-8ad0-1abb9fedcfbf.pdf
                    var filePath = Path.Combine(folderPath, $"{batch.Id}.{extension}");

                    // 3. Handle Overwrite (If file already exists for this batch/type)
                    if (File.Exists(filePath)) File.Delete(filePath);

                    var existingRecord = batch.Files.FirstOrDefault(f => f.DocumentType == item.DocType);
                    if (existingRecord != null) _context.LoanFileItem.Remove(existingRecord);

                    // 4. Save Physical File
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.File.CopyToAsync(stream);
                    }
                    newlyCreatedFiles.Add(filePath);

                    // 5. Add DB Record (Note: Item ID is still unique for DB integrity, 
                    // but the physical file on disk uses the Batch.Id)
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
                return batch.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                foreach (var path in newlyCreatedFiles) if (File.Exists(path)) File.Delete(path);
                throw;
            }
        }

        public async Task<List<FileViewDto>> GetAllFilesByBatchIdAsync(Guid batchId)
        {
            var files = await _context.LoanFileItem
                .Where(f => f.FileBatchId == batchId)
                .ToListAsync();

            return files.Select(f => new FileViewDto
            {
                FileId = f.Id,
                FileName = f.FileName,
                DocType = f.DocumentType.ToString(),
                // Map the ContentType based on extension
                ContentType = GetMimeType(f.FileFormat),
            }).ToList();
        }

        private string GetMimeType(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension)) return "application/octet-stream";

            // Strip leading dot if present
            extension = extension.TrimStart('.').ToLower();

            return extension switch
            {
                "pdf" => "application/pdf",
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "tiff" or "tif" => "image/tiff",
                "txt" => "text/plain",
                "csv" => "text/csv",
                "xls" => "application/vnd.ms-excel",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "doc" => "application/msword",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "zip" => "application/zip",
                _ => "application/octet-stream" // Forces a download if type is unknown
            };
        }


    }
}

