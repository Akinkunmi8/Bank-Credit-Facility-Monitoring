using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class AddFiledetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomDocumentType",
                table: "LoanFileItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFileVerified",
                table: "LoanFileItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomDocumentType",
                table: "LoanFileItem");

            migrationBuilder.DropColumn(
                name: "IsFileVerified",
                table: "LoanFileItem");
        }
    }
}
