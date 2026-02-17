using AutoMapper;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.Model;

namespace BankLoanMonetrySolu.Logic
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FinacleData,FinacleDataDto>();
            CreateMap<FinacleData, FincaleCompareDataDto>();
            CreateMap<CreditAvailmentTicket,CreditAvailmentTicketDto>();
            CreateMap<CustomerDetails,CustomerDetailsDto>();
            CreateMap<CreditAvailmentTicketDto, CompareCatDto>();
            CreateMap<FinacleData, UnresolvedTATDetailDto>();

        }
    }
}
