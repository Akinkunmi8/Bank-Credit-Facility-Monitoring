using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "customerDetails",
                columns: new[] { "Id", "Age", "ApplicantType", "DOB", "Email", "Firstname", "Lastname", "MaritalStatus", "Middlename", "NameOfEmployer", "Nationallity", "NumbOfDependant", "ResidentStatus", "Tittle", "gender", "phoneNum" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Individual", "21-05-1989", "john.doe@test.com", "John", "Doe", "Married", null, "UBa", "Nigerian", "2", "Resident", "Mr", "Male", "08030000001" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Individual", "05-07-1999", "mary.j@test.com", "Mary", "Johnson", "Married", null, "Wema", "Nigerian", "2", "Resident", "Mrs", "Female", "08030000002" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Individual", "12-05-1994", "ahmed.s@test.com", "Ahmed", "Sule", "Married", null, "Unilag", "Nigerian", "3", "Resident", "Mr", "Male", "08030000003" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, "Individual", "11-09-2000", "blessing.o@test.com", "Blessing", "Okoro", "Single", null, "Unilever", "Nigerian", "2", "Resident", "Ms", "Famale", "08030000004" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
