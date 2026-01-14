using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Logic;

namespace BankLoanMonetrySolu.Services
{
    using Microsoft.EntityFrameworkCore;

    public class TATService : ITAT
    {
        private readonly DALContext _context;

        public TATService(DALContext context)
        {
            _context = context;
        }

        public async Task<TATResponseDto> GetWithinTAT(string region)
        {
            var count = await _context.finacleDatas
                .Where(x => x.PROCESSEDATE >= DateTime.UtcNow.AddHours(-2) && x.REGION == region)
                .CountAsync();

            return new TATResponseDto
            {
                IsSuccessful = true,
                TATCount = count.ToString()
            };
        }

        public async Task<TATResponseDto> GetBeyondTAT(string region)
        {
            var count = await _context.finacleDatas
                .Where(x => x.PROCESSEDATE < DateTime.UtcNow.AddHours(-2) && x.CONFIRMED == false && x.REGION == region)
                .CountAsync();

            return new TATResponseDto
            {
                IsSuccessful = true,
                TATCount = count.ToString()
            };
        }
    }


}
