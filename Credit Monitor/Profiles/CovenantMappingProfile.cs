using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModels;

namespace Credit_Monitor.Mappings
{
    public class CovenantMappingProfile : Profile
    {
        public CovenantMappingProfile()
        {
            CreateMap<FinancialCovenantDto, FinancialCovenantVM>();
            CreateMap<NonFinancialCovenantDto, NonFinancialCovenantVM>();

            // Simplified mapping: Financials maps directly to Financials now
            CreateMap<CovenantResponseDto, CovenantViewModel>();
        }
    }
}