using BankLoanMonetrySolu.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankLoanMonetrySolu.Context
{
    public class DALContext : IdentityDbContext<AppUsers>
    {
        public DALContext(DbContextOptions<DALContext> options)
            : base(options)
        {
        }

        // Your existing tables
        public DbSet<FinacleData> finacleDatas { get; set; }
        public DbSet<CreditAvailmentTicket> cat { get; set; }
        public DbSet<CustomerDetails> customerDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1️⃣ MUST call base for Identity
            base.OnModelCreating(modelBuilder);

            // 2️⃣ Configure your existing relationships
            modelBuilder.Entity<CustomerDetails>()
                .HasOne(c => c.loanDetails)
                .WithOne(f => f.Customer)
                .HasForeignKey<FinacleData>(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3️⃣ Seed your existing data
            modelBuilder.Entity<CustomerDetails>().HasData(
                new CustomerDetails
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Tittle = "Mr",
                    Firstname = "John",
                    Lastname = "Doe",
                    Email = "john.doe@test.com",
                    phoneNum = "08030000001",
                    DOB = "21-05-1989",
                    MaritalStatus = "Married",
                    NumbOfDependant = "2",
                    gender = "Male",
                    ResidentStatus = "Resident",
                    NameOfEmployer = "UBa",
                    Nationallity = "Nigerian",
                    ApplicantType = "Individual"
                },
                new CustomerDetails
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Tittle = "Mrs",
                    Firstname = "Mary",
                    Lastname = "Johnson",
                    Email = "mary.j@test.com",
                    phoneNum = "08030000002",
                    DOB = "05-07-1999",
                    MaritalStatus = "Married",
                    NumbOfDependant = "2",
                    gender = "Female",
                    ResidentStatus = "Resident",
                    NameOfEmployer = "Wema",
                    Nationallity = "Nigerian",
                    ApplicantType = "Individual"
                },
                new CustomerDetails
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Tittle = "Mr",
                    Firstname = "Ahmed",
                    Lastname = "Sule",
                    Email = "ahmed.s@test.com",
                    phoneNum = "08030000003",
                    DOB = "12-05-1994",
                    MaritalStatus = "Married",
                    NumbOfDependant = "3",
                    gender = "Male",
                    ResidentStatus = "Resident",
                    NameOfEmployer = "Unilag",
                    Nationallity = "Nigerian",
                    ApplicantType = "Individual"
                },
                new CustomerDetails
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Tittle = "Ms",
                    Firstname = "Blessing",
                    Lastname = "Okoro",
                    Email = "blessing.o@test.com",
                    phoneNum = "08030000004",
                    DOB = "11-09-2000",
                    MaritalStatus = "Single",
                    NumbOfDependant = "2",
                    gender = "Female",
                    ResidentStatus = "Resident",
                    NameOfEmployer = "Unilever",
                    Nationallity = "Nigerian",
                    ApplicantType = "Individual"
                }
            );

            // 4️⃣ Rename Identity tables to your preferred names
            modelBuilder.Entity<AppUsers>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // Optional: configure indexes, max lengths, etc. here
        }
    }
}


