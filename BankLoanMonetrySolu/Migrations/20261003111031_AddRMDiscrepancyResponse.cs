using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class AddRMDiscrepancyResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolutionAction",
                table: "discrepancyRemarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionRemarks",
                table: "discrepancyRemarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RespondedAt",
                table: "discrepancyRemarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespondedBy",
                table: "discrepancyRemarks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolutionAction",
                table: "discrepancyRemarks");

            migrationBuilder.DropColumn(
                name: "ResolutionRemarks",
                table: "discrepancyRemarks");

            migrationBuilder.DropColumn(
                name: "RespondedAt",
                table: "discrepancyRemarks");

            migrationBuilder.DropColumn(
                name: "RespondedBy",
                table: "discrepancyRemarks");
        }
    }
}
