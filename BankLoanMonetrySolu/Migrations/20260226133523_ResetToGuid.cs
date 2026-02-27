using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class ResetToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Only create the table that is missing
            migrationBuilder.CreateTable(
                name: "discrepancyRemarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discrepancies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecomendedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discrepancyRemarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_discrepancyRemarks_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks",
                column: "AppId"); // No 'unique: true' here!
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ONLY drop the table we created in the Up method
            migrationBuilder.DropTable(
                name: "discrepancyRemarks");
        }
    }
}
