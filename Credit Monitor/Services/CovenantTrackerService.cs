using AutoMapper;
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
    }
}