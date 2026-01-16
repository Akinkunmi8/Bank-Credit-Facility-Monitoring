using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class CreateFileTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoanFileBatchId",
                table: "finacleDatas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoanFileBatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanFileBatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanFileItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanFileItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanFileItem_LoanFileBatch_FileBatchId",
                        column: x => x.FileBatchId,
                        principalTable: "LoanFileBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_finacleDatas_LoanFileBatchId",
                table: "finacleDatas",
                column: "LoanFileBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileBatch_AppId",
                table: "LoanFileBatch",
                column: "AppId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileItem_FileBatchId",
                table: "LoanFileItem",
                column: "FileBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_finacleDatas_LoanFileBatch_LoanFileBatchId",
                table: "finacleDatas",
                column: "LoanFileBatchId",
                principalTable: "LoanFileBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_finacleDatas_LoanFileBatch_LoanFileBatchId",
                table: "finacleDatas");

            migrationBuilder.DropTable(
                name: "LoanFileItem");

            migrationBuilder.DropTable(
                name: "LoanFileBatch");

            migrationBuilder.DropIndex(
                name: "IX_finacleDatas_LoanFileBatchId",
                table: "finacleDatas");

            migrationBuilder.DropColumn(
                name: "LoanFileBatchId",
                table: "finacleDatas");
        }

    }
}
