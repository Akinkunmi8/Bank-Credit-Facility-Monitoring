using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class CAT : ICAT
    {
        private readonly DALContext _context;
        private readonly IMapper _mapper;

        public CAT(DALContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CATResponseDto> CreateCatAsync(CreditAvailmentTicketDto catDto)
        {
            try
            {
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

                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "CAT created successfully"
                };
            }
            catch (Exception ex)
            {
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
                var cats = await _context.cat.ToListAsync();
                var dto = _mapper.Map<List<CreditAvailmentTicketDto>>(cats);
                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = $"{cats.Count} CAT records retrieved",
                    DataList = dto
                };
            }
            catch (Exception ex)
            {
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
                var cat = await _context.cat
                    .FirstOrDefaultAsync(c => c.AccNum == accountNum);
                var dto = _mapper.Map<CreditAvailmentTicketDto>(cat);
                if (cat == null)
                    return new CATResponseDto
                    {
                        IsSuccess = false,
                        ResponseMessage = $"No CAT found for account number {accountNum}"
                    };

                return new CATResponseDto
                {
                    IsSuccess = true,
                    ResponseMessage = "CAT retrieved successfully",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
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
                // Select only the fields you need
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
                    .FirstOrDefaultAsync(); // get single record

                if (data == null)
                {
                    return new CompareCatDto
                    {
                        IsSuccess = false,
                        ResponseMessage = "No record found for the given account number"
                    };
                }

                // Map to CompareCatDto
                var result = _mapper.Map<CompareCatDto>(data);
                result.IsSuccess = true;
                result.ResponseMessage = "Record retrieved successfully";

                return result;
            }
            catch (Exception ex)
            {
                return new CompareCatDto
                {
                    IsSuccess = false,
                    ResponseMessage = $"Failed to fetch record: {ex.Message}"
                };
            }
        }


    }

}

