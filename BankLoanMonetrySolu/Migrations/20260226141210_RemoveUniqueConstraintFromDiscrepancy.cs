using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromDiscrepancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old unique one
            migrationBuilder.DropIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks");

            // Create the new non-unique one
            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks",
                column: "AppId",
                unique: false); // Set to false!
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
        name: "IX_discrepancyRemarks_AppId",
        table: "discrepancyRemarks");

            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks",
                column: "AppId",
                unique: true);
        }
    }
}
