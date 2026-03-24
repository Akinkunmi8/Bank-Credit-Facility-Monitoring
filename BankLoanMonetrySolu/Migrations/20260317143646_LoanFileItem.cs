using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class LoanFileItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppId",
                table: "LoanFileItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileItem_AppId",
                table: "LoanFileItem",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanFileItem_finacleDatas_AppId",
                table: "LoanFileItem",
                column: "AppId",
                principalTable: "finacleDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanFileItem_finacleDatas_AppId",
                table: "LoanFileItem");

            migrationBuilder.DropIndex(
                name: "IX_LoanFileItem_AppId",
                table: "LoanFileItem");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "LoanFileItem");
        }
    }
}
