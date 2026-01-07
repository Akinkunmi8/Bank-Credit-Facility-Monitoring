using AutoMapper;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        public LoanService(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }
        public async Task<PendingConfirmedLoanViewModel?> GetPendingConfirmedLoanByIdAsync(string id)
        {
            var loan = await _loanRepository.GetPendingConfirmedLoanByIdAsync(id);
            if (loan == null) return null;
            return _mapper.Map<PendingConfirmedLoanViewModel>(loan);
        }

        public async Task<IEnumerable<PendingConfirmedLoanViewModel>> GetPendingConfirmedLoansByRegion(string region)
        {
            var loans = await _loanRepository.GetPendingConfirmedLoansByRegionAsync(region);
            return _mapper.Map<IEnumerable<PendingConfirmedLoanViewModel>>(loans);
        }
    }
}
