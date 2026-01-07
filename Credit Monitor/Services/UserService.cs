using Credit_Monitor.Interfaces;
using Credit_Monitor.Models;
using Credit_Monitor.Data;
using Credit_Monitor.ViewModel;
using System.Runtime.CompilerServices;

namespace Credit_Monitor.Services
{
    public class UserService : IUserService
    {
        public Task<LoginResponseViewModel> Authenticate(string username, string password)
        {
            var user = InMemoryUsers.Users
        .FirstOrDefault(u => u.Username == username && u.Password == password);


            if (user == null)
            {              
                return Task.FromResult(GetResponse("Invalid username or password.", "Failed", null));
            }
            var UserVM = new UserViewModel
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Region = user.Region
            };
            return Task.FromResult(GetResponse("Login successful!", "Success", UserVM));
        }

        private static LoginResponseViewModel GetResponse(string message, string status, UserViewModel? UserVM )
        {
            return new LoginResponseViewModel
            {
                Response = new ResponseViewModel
                {
                    Message = message,
                    Status = status
                },
                User = UserVM
            };
        }

    }
}
