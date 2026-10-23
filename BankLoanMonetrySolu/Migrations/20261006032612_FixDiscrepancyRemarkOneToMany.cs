using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class FixDiscrepancyRemarkOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks");

            migrationBuilder.AlterColumn<string>(
                name: "Discrepancies",
                table: "discrepancyRemarks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId_Discrepancies",
                table: "discrepancyRemarks",
                columns: new[] { "AppId", "Discrepancies" },
                unique: true,
                filter: "[Discrepancies] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_discrepancyRemarks_AppId_Discrepancies",
                table: "discrepancyRemarks");

            migrationBuilder.AlterColumn<string>(
                name: "Discrepancies",
                table: "discrepancyRemarks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks",
                column: "AppId",
                unique: true);
        }
    }
}
