using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Added for NLog
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class CAT : ICAT
    {
        private readonly DALContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CAT> _logger; // 1. Define Logger

        public CAT(DALContext context, IMapper mapper, ILogger<CAT> logger) // 2. Inject Logger
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CATResponseDto> CreateCatAsync(CreditAvailmentTicketDto catDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create CAT for Customer: {CustomerName}, Account: {AccNum}", catDto.CustomerName, catDto.AccNum);

                var cat = new CreditAvailmentTicket
                {
                    CustomerName = catDto.CustomerName,
                    AccNum = catDto.AccNum,
                    CreditDate = catDto.CreditDate,
                    Directorate = catDto.Directorate,
                    SchemeCode = catDto.SchemeCode,
                    AccountOfficer = catDto.AccountOfficer,
                    ProductPaperType = catDto.ProductPaperType,
                    currentOutstanding = catDto.currentOutstanding,
                    DisbursementRequired = catDto.DisbursementRequired,
                    OutstandingAfterDisbursement = catDto.OutstandingAfterDisbursement,
                    ApprovalLimit = catDto.ApprovalLimit,
                    Interest = catDto.interest,
                    Fees = catDto.Fees,
                    ApprovalLevel = catDto.ApprovalLevel,
                    TypeOfApproval = catDto.TypeOfApproval,
                    FacilityType = catDto.FacilityType,
                    Valuedate = catDto.Valuedate,
                    MaturityOrExpiry = catDto.MaturityOrExpiry
                };

                await _context.cat.AddAsync(cat);
                await _context.SaveChangesAsync();

                _logger.LogInformation("CAT created successfully for Account: {AccNum}. Approval Limit: {Limit}", catDto.AccNum, catDto.ApprovalLimit);

                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "CAT created successfully"
                };
            }
            catch (Exception ex)
            {
                // Capture the full exception for debugging
                _logger.LogError(ex, "Error occurred while creating CAT for Account: {AccNum}", catDto.AccNum);

                return new CATResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error creating CAT: {ex.Message}"
                };
            }
        }

        public async Task<CATResponseDto> GetAllCAT()
        {
            try
            {
                _logger.LogInformation("Fetching all CAT records from database.");

                var cats = await _context.cat.ToListAsync();
                var dto = _mapper.Map<List<CreditAvailmentTicketDto>>(cats);

                _logger.LogInformation("Successfully retrieved {Count} CAT records.", cats.Count);

                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{cats.Count} CAT records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all CAT records.");
                return new CATResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching CATs: {ex.Message}"
                };
            }
        }

        public async Task<CATResponseDto> GetCATByAccountNum(string accountNum)
        {
            try
            {
                _logger.LogInformation("Searching for CAT with Account Number: {AccNum}", accountNum);

                var cat = await _context.cat.FirstOrDefaultAsync(c => c.AccNum == accountNum);

                if (cat == null)
                {
                    _logger.LogWarning("CAT lookup failed: No record found for Account Number: {AccNum}", accountNum);
                    return new CATResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = $"No CAT found for account number {accountNum}"
                    };
                }

                var dto = _mapper.Map<CreditAvailmentTicketDto>(cat);
                _logger.LogInformation("CAT retrieved successfully for Account: {AccNum}", accountNum);

                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "CAT retrieved successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving CAT for Account: {AccNum}", accountNum);
                return new CATResponseDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error fetching CAT: {ex.Message}"
                };
            }
        }

        public async Task<CompareCatDto> GetCompareCatdata(string accountNum)
        {
            try
            {
                _logger.LogInformation("Initiating CAT data comparison for Account: {AccNum}", accountNum);

                var data = await _context.cat
                    .Where(c => c.AccNum == accountNum)
                    .Select(c => new CreditAvailmentTicketDto
                    {
                        CustomerName = c.CustomerName,
                        AccNum = c.AccNum,
                        Directorate = c.Directorate,
                        ProductPaperType = c.ProductPaperType,
                        DisbursementRequired = c.DisbursementRequired,
                        ApprovalLimit = c.ApprovalLimit,
                        interest = c.Interest,
                        Fees = c.Fees,
                        Valuedate = c.Valuedate,
                        MaturityOrExpiry = c.MaturityOrExpiry
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    _logger.LogWarning("Comparison data lookup failed: Account {AccNum} not found.", accountNum);
                    return new CompareCatDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "No record found for the given account number"
                    };
                }

                var result = _mapper.Map<CompareCatDto>(data);
                result.IsSuccess = true;
                result.ResponseMessage = "Record retrieved successfully";

                _logger.LogInformation("Comparison data successfully mapped for Account: {AccNum}", accountNum);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comparison data for Account: {AccNum}", accountNum);
                return new CompareCatDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Failed to fetch record: {ex.Message}"
                };
            }
        }
    }
}