using AutoMapper;
using Credit_Monitor.APIDto;
using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Profiles
{
    public class RegisterProfile : Profile
    {
          public RegisterProfile()
        {
            CreateMap<RegisterViewModel, RegisterRequestDto>();
            // Add other maps here
        }
    }
}