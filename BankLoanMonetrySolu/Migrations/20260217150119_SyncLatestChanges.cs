using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class SyncLatestChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "discrepancyRemarks",
                columns: table => new
                {
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discrepancies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecomendedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsibility = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discrepancyRemarks", x => x.AppId);
                    table.ForeignKey(
                        name: "FK_discrepancyRemarks_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discrepancyRemarks");
        }
    }
}
