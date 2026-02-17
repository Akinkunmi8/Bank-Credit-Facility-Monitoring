using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class addNewRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "856a6b9f-3c52-493f-ba9e-6009bfaa5002", null, "AO", "AO" },
                    { "bc5128c5-78c3-47b7-be0e-b1b3fcc37f4c", null, "BM", "BM" },
                    { "e0f2d69d-1afe-49a6-8633-bcae374e604f", null, "CMS", "CMS" },
                    { "e35f8fdb-4566-4f48-b565-5edf7a0bc205", null, "CMO", "CMO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "856a6b9f-3c52-493f-ba9e-6009bfaa5002");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bc5128c5-78c3-47b7-be0e-b1b3fcc37f4c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e0f2d69d-1afe-49a6-8633-bcae374e604f");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e35f8fdb-4566-4f48-b565-5edf7a0bc205");
        }
    }
}
