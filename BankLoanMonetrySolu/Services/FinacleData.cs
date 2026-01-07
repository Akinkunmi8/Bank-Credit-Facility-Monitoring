using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class FinacleData : IFinacleData
    {
        private readonly DALContext _context;
        private readonly IMapper _mapper;

        public FinacleData(DALContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                    .Where(f => f.REGION == region && f.RECONFIRMED == true && f.RECONFIRMED == false && f.APPROVED == false)
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
                    .Where(f => f.REGION == region && f.CONFIRMED == true  && f.RECONFIRMED == false && f.APPROVED == false ) // Example filter
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
                    .Where(f => f.REGION_NAME == region && f.CONFIRMED == false  && f.RECONFIRMED == false && f.APPROVED == false)
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
                    f.APPROVED == false
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
                    f.CONFIRMED == true &&
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
            // 1️⃣ Convert string ApplicationId to Guid
            if (!Guid.TryParse(dto.ApplicationId, out Guid appGuid))
                return false; // invalid ApplicationId

            // 2️⃣ Fetch the record
            var record = await _context.finacleDatas
                .FirstOrDefaultAsync(x => x.Id == appGuid);

            if (record == null)
                return false; // record not found

            // 3️⃣ Current timestamp for TAT
            var actionTime = DateTime.Now;

            // 4️⃣ Update based on action
            switch (dto.Action?.ToLower())
            {
                case "confirm":
                    record.CONFIRMED = true;
                    record.PROCESSEDATE = actionTime; // timestamp for TAT
                    break;

                case "reconfirm":
                    record.RECONFIRMED = true;
                    record.PROCESSEDATE = actionTime;
                    break;

                case "approve":
                    record.APPROVED = true;
                    record.PROCESSEDATE = actionTime;
                    break;

                case "reject":
                    record.RECONFIRMED = true; // or a REJECTED flag if you have one
                    record.PROCESSEDATE = actionTime;
                    break;

                default:
                    throw new ArgumentException($"Invalid action: {dto.Action}");
            }

            // 5️⃣ Save changes
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

    }
}
