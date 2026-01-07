using Credit_Monitor.ViewModel;

namespace Credit_Monitor.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponseViewModel> Authenticate(string username, string password);
    }

}
