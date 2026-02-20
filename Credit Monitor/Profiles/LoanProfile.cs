using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.APIsDto;
using Credit_Monitor.Models;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile() {
            // CreateMap<Source, Destination>();
            // If property names are identical, AutoMapper handles them automatically.
            CreateMap<PendingConfirmedLoan, PendingConfirmedLoanViewModel>();
            CreateMap<TATOutputResponse, PendingConfirmedLoanViewModel>();
                 //.ForMember(dest => dest.accounT_NAME, opt => opt.MapFrom(src => src.AccountName));
            CreateMap<CATViewModel, CAT>().ReverseMap();
            CreateMap<FinacleRecord, PendingConfirmedLoanViewModel>();
            CreateMap<CustomerDetailsDto, CustomerDetailsViewModel>();
            CreateMap<FinacleLoanAndCustomerDto, PendingConfirmedLoanViewModel>();
            CreateMap<FinacleDataResponseDto, FinacleDataViewModel>();
            CreateMap<CatDataDto, CATViewModel>();

            // If you had different names, you would use:
            // .ForMember(dest => dest.accounT_NAME, opt => opt.MapFrom(src => src.FullName));
        }

    }
}
