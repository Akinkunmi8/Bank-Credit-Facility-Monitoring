using Credit_Monitor.Models;

namespace Credit_Monitor.Data
{
    public class InMemoryUsers
    {
        public static List<User> Users = new()
        {
            new User
            {
                Id = 1,
                Username = "staff_sw",
                Password = "123",
                Role = UserRole.Staff,
                Region = "L2NG010145"
            },
            new User
            {
                Id = 2,
                Username = "staff_se",
                Password = "123",
                Role = UserRole.Staff,
                Region = "south-east"
            },
            new User
            {
                Id = 3,
                Username = "supervisor",
                Password = "123",
                Role = UserRole.Supervisor,
                Region = "north-central"
            }
        };
    }
}
