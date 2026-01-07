using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigrationToAddCustomerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "APPROVED",
                table: "finacleDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CONFIRMED",
                table: "finacleDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "finacleDatas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PROCESSEDATE",
                table: "finacleDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RECONFIRMED",
                table: "finacleDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "customerDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tittle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Middlename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumbOfDependant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationallity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phoneNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfEmployer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_finacleDatas_CustomerId",
                table: "finacleDatas",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_finacleDatas_customerDetails_CustomerId",
                table: "finacleDatas",
                column: "CustomerId",
                principalTable: "customerDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_finacleDatas_customerDetails_CustomerId",
                table: "finacleDatas");

            migrationBuilder.DropTable(
                name: "customerDetails");

            migrationBuilder.DropIndex(
                name: "IX_finacleDatas_CustomerId",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "APPROVED",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "CONFIRMED",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "PROCESSEDATE",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "RECONFIRMED",
                table: "finacleDatas");
        }
    }
}
