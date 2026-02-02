using BankLoanMonetrySolu.Model;
using BankLoanMonetrySolu.Models;
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

        public DbSet<FinacleData> finacleDatas { get; set; }
        public DbSet<CreditAvailmentTicket> cat { get; set; }
        public DbSet<CustomerDetails> customerDetails { get; set; }
        public DbSet<LoanFileBatch> LoanFileBatch { get; set; }
        public DbSet<LoanFileItem> LoanFileItem { get; set; }
        public DbSet<Audit> audits { get; set; }
        public DbSet<FinancialCovenant> financialCovenants { get; set; }
        public DbSet<NonFinancialCovenant> nonFinancialCovenants{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1️⃣ MUST call base for Identity
            base.OnModelCreating(modelBuilder);

            // 2️⃣ FIX FOR DECIMAL WARNINGS
            // Automatically sets all decimal properties to (18,2) to avoid truncation warnings
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // Customer ↔ FinacleData (1–1)
            modelBuilder.Entity<CustomerDetails>()
                .HasOne(c => c.loanDetails)
                .WithOne(f => f.Customer)
                .HasForeignKey<FinacleData>(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // LoanFileBatch uniqueness per AppId
            modelBuilder.Entity<LoanFileBatch>()
                .HasIndex(x => x.AppId)
                .IsUnique();

            // LoanFileBatch ↔ LoanFileItem (1–Many)
            modelBuilder.Entity<LoanFileItem>()
                .HasOne(x => x.FileBatch)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.FileBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            // FinacleData ↔ LoanFileBatch
            modelBuilder.Entity<FinacleData>()
                .HasOne(f => f.LoanFileBatch)
                .WithMany()
                .HasForeignKey(f => f.LoanFileBatchId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.FinacleData)
                      .WithMany()
                      .HasForeignKey(x => x.AppId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // Financial Covenant Configuration
            modelBuilder.Entity<FinancialCovenant>(entity =>
            {
                entity.HasKey(x => x.Id);

                // Relationship: FinacleData ↔ FinancialCovenants (1-Many)
                entity.HasOne<FinacleData>()
                      .WithMany()
                      .HasForeignKey(x => x.AppId)
                      .HasPrincipalKey(f => f.Id) // Linking via AppId as the unique anchor
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure precision for Financial Ratios (e.g., 1.20x)
                entity.Property(x => x.TargetValue)
                      .HasPrecision(18, 2);

                // Convert Enum to String in DB for readability
                entity.Property(x => x.Frequency)
                      .HasConversion<string>();
            });

            // Non-Financial Covenant Configuration
            modelBuilder.Entity<NonFinancialCovenant>(entity =>
            {
                entity.HasKey(x => x.Id);

                // Relationship: FinacleData ↔ NonFinancialCovenants (1-Many)
                entity.HasOne<FinacleData>()
                      .WithMany()
                      .HasForeignKey(x => x.AppId)
                      .HasPrincipalKey(f => f.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Convert Enum to String in DB
                entity.Property(x => x.Frequency)
                      .HasConversion<string>();
            });



            //// 4️⃣ Seed Data for Testing Validation
            //var customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            //modelBuilder.Entity<CustomerDetails>().HasData(
            //    new CustomerDetails
            //    {
            //        Id = customerId,
            //        Tittle = "Mr",
            //        Firstname = "John",
            //        Lastname = "Doe",
            //        Email = "john.doe@test.com",
            //        phoneNum = "08030000001",
            //        DOB = "21-05-1989",
            //        MaritalStatus = "Married",
            //        gender = "Male",
            //        ResidentStatus = "Resident",
            //        NameOfEmployer = "UBA",
            //        Nationallity = "Nigerian",
            //        ApplicantType = "Individual"
            //    }
            //);

            //// Seed Finacle (Core Banking Side)
            //modelBuilder.Entity<FinacleData>().HasData(
            //    new FinacleData
            //    {
            //        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //        ACCOUNT_NUMBER = "1234567890",
            //        ACCOUNT_NAME = "John Doe",
            //        SANCTION_LIMIT = 5000000m,
            //        INT_RATE = 15.5m,
            //        EXPIRY_DATE = new DateTime(2027, 12, 31),
            //        CustomerId = customerId,
            //        SBU_NAME = "RETAIL BANKING",
            //        FACILITY_TYPE = "Personal Loan"
            //    }
            //);

            //// Seed CAT (Internal Application Side)
            //modelBuilder.Entity<CreditAvailmentTicket>().HasData(
            //    new CreditAvailmentTicket
            //    {
            //        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //        AccNum = "1234567890",
            //        CustomerName = "John Doe", // Match
            //        ApprovalLimit = 4500000m,  // DISCREPANCY: 5.0M vs 4.5M
            //        Interest = 15.5m,         // Match
            //        MaturityOrExpiry = new DateTime(2027, 12, 31), // Match
            //        Directorate = "RETAIL BANKING"
            //    }
            //);

            // 5️⃣ Rename Identity tables
            modelBuilder.Entity<AppUsers>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}