using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Mappings
{
    public class CovenantMappingProfile : Profile
    {
        public CovenantMappingProfile()
        {
            CreateMap<FinancialCovenantDto, FinancialCovenantVM>();
            CreateMap<NonFinancialCovenantDto, NonFinancialCovenantVM>();
            CreateMap<NonFinancialCovenantVM, UpdateCovenantRequestDto>()
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => (int)src.Frequency));

            // Simplified mapping: Financials maps directly to Financials now
            CreateMap<CovenantResponseDto, CovenantViewModel>();
        }
    }
}