using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.Interfaces;
using Credit_Monitor.ViewModels;

namespace Credit_Monitor.Services
{
    public class CovenantTrackerService : ICovenantTrackerService
    {
        private readonly ICovenantTrackerAPIService _covenantAPIService;
        private readonly IMapper _mapper;

        public CovenantTrackerService(ICovenantTrackerAPIService covenantAPIService, IMapper mapper)
        {
            _covenantAPIService = covenantAPIService;
            _mapper = mapper;
        }

        public async Task<CovenantViewModel?> GetCovenantDetailsAsync(Guid appId)
        {
            try
            {
                var covenantDto = await _covenantAPIService.GetCovenantDetails(appId);

                if (covenantDto == null)
                {
                    return null;
                }

                var viewModel = _mapper.Map<CovenantViewModel>(covenantDto);

                return viewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UpdateCovenantResponseDto> UpdateCovenant(NonFinancialCovenantVM model, Guid appId)
        {
            try
            {

                var requestDto = _mapper.Map<UpdateCovenantRequestDto>(model);

                var response = await _covenantAPIService.UpdateNonFinancialCovenant(appId, requestDto);

                return response!;
            }
            catch (Exception ex)
            {
 
                return new UpdateCovenantResponseDto
                {
                    Message = $"Service Layer Error: {ex.Message}"
                };
            }
        }

        public async Task<UpdateCovenantResponseDto> CreateCovenant(NonFinancialCovenantVM model, Guid appId)
        {
            try
            {
                var requestDto = _mapper.Map<UpdateCovenantRequestDto>(model);

                var response = await _covenantAPIService.AddNonFinancialCovenant(appId, requestDto);

                return response ?? new UpdateCovenantResponseDto { Message = "API returned no response." };
            }
            catch (Exception ex)
            {
                return new UpdateCovenantResponseDto
                {
                    Message = $"Service Layer Error: {ex.Message}"
                };
            }
        }
    }
}