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
            CreateMap<CreditAvailmentTicket,CreditAvailmentTicketDto>();
            CreateMap<CustomerDetails,CustomerDetailsDto>();
        }
    }
}
