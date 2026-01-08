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
            CreateMap<FinacleDataDto, FincaleCompareDataDto>();
            CreateMap<CreditAvailmentTicket,CreditAvailmentTicketDto>();
            CreateMap<CustomerDetails,CustomerDetailsDto>();
            CreateMap<CreditAvailmentTicketDto, CompareCatDto>();

        }
    }
}
