using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class SetDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "cat",
                columns: new[] { "Id", "AccNum", "AccountOfficer", "ApprovalLevel", "ApprovalLimit", "CreditDate", "CustomerName", "Directorate", "DisbursementRequired", "FacilityType", "Fees", "MaturityOrExpiry", "OutstandingAfterDisbursement", "ProductPaperType", "SchemeCode", "TypeOfApproval", "Valuedate", "currentOutstanding", "interest" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "1234567890", null, null, 4500000m, null, "John Doe", "RETAIL BANKING", null, null, null, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "15.5" });

            migrationBuilder.UpdateData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "NameOfEmployer", "NumbOfDependant" },
                values: new object[] { "UBA", null });

            migrationBuilder.InsertData(
                table: "finacleDatas",
                columns: new[] { "Id", "ACCOUNT_CURRENCY", "ACCOUNT_NAME", "ACCOUNT_NUMBER", "ACCOUNT_OFFICER", "ACCT_CRNCY_CODE", "APPROVED", "BANK_HEAD", "BANK_NAME", "BM_STAFF_NAME", "BOM_STAFF_NAME", "BRANCH", "BRANCH_NAME", "COLLATERAL_DESCRIPTION", "COLLATERAL_VALUE", "CONFIRMED", "CONTRACT_ACID", "CONTRACT_ID", "COUNTRY", "CUM_INTEREST_DUE_NOT_YET_PAID", "CUM_INTEREST_REPAYMENT_AMOUNT_PAID", "CUM_PRINCIPAL_DUE_NOT_YET_PAID", "CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID", "CUM_REPAYMENT_AMOUNT_DUE", "CUM_REPAYMENT_AMOUNT_PAID", "CUSTOMER_ID", "CUST_SECTOR", "CUST_SECTOR_DESC", "CUST_SUB_SECTOR", "CUST_SUB_SECTOR_DESC", "CustomerId", "DATE_GRANTED", "DATE_PAST_DUE", "DAYS_PAST_DUE", "DETAILS_OF_SECURITIES_OTHERS", "DIRECTORATE", "DIRECTORATE_HEAD", "DIRECTORATE_NAME", "EXPIRY_DATE", "FACILITY_TYPE", "FLOW_START_DATE", "FX_RATE", "GL_ASSET", "GROUP_ID", "IENC", "INSTALMENT", "INT_COLL_BACID", "INT_RATE", "INT_REPAYMENT_FREQUENCY", "LAST_CREDIT_AMT", "LAST_CREDIT_DATE", "LE_BOOK", "LOAN_BALANCE_FCY", "LOAN_BALANCE_LCY", "NEWCUM_REPAYMENT_AMOUNT_DUE", "NEXT_DMD_DATE", "NEXT_INTEREST_AMOUNT", "NEXT_PRINCIPAL_AMOUNT", "NEXT_REP_INT_DATE", "OPERATIVE_ACCOUNT_CURRENCY", "OPERATIVE_ACCT_BALANCE_FCY", "OPERATIVE_ACCT_BALANCE_LCY", "OVERDUE_AMT", "PERFORMANCE_CLASS", "PERFORMANCE_CLASS_DESC", "PIENC", "PRIN_REPAYMENT_FREQUENCY", "PROCESSEDATE", "RECONFIRMED", "REGION", "REGION_HEAD", "REGION_NAME", "RISKRATING", "RISK_ASSET_TYPE", "ROWNUMBER_ORDERBY", "SANCTION_LIMIT", "SBU_NAME", "SBU_TYPE", "SCHM_CODE", "SOL_ID", "TENOR_DAYS", "TENOR_MONTH", "TOD_LIMIT", "TOTAL_BALANCE_FCY", "TOTAL_BALANCE_LCY", "TOTAL_COLLECTION_12M", "TOTAL_COLLECTION_3M", "TOTAL_COLLECTION_6M", "TRANSACTION_DATE", "YEAR_MONTH" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, "John Doe", "1234567890", null, null, false, null, null, null, null, null, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, null, null, null, null, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personal Loan", null, 0, null, null, null, null, null, 15.5m, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, null, null, null, null, null, null, 5000000m, "RETAIL BANKING", null, null, null, null, null, 0, null, null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DeleteData(
                table: "cat",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "finacleDatas",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.UpdateData(
                table: "customerDetails",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "NameOfEmployer", "NumbOfDependant" },
                values: new object[] { "UBa", "2" });

            migrationBuilder.InsertData(
                table: "customerDetails",
                columns: new[] { "Id", "Age", "ApplicantType", "DOB", "Email", "Firstname", "Lastname", "MaritalStatus", "Middlename", "NameOfEmployer", "Nationallity", "NumbOfDependant", "ResidentStatus", "Tittle", "gender", "phoneNum" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Individual", "05-07-1999", "mary.j@test.com", "Mary", "Johnson", "Married", null, "Wema", "Nigerian", "2", "Resident", "Mrs", "Female", "08030000002" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Individual", "12-05-1994", "ahmed.s@test.com", "Ahmed", "Sule", "Married", null, "Unilag", "Nigerian", "3", "Resident", "Mr", "Male", "08030000003" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, "Individual", "11-09-2000", "blessing.o@test.com", "Blessing", "Okoro", "Single", null, "Unilever", "Nigerian", "2", "Resident", "Ms", "Famale", "08030000004" }
                });
        }
    }
}
