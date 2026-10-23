using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class FreshInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Directorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPaperType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentOutstanding = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisbursementRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingAfterDisbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfApproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valuedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityOrExpiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat", x => x.Id);
                });

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
                name: "finacleDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    COUNTRY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LE_BOOK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YEAR_MONTH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RISK_ASSET_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONTRACT_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONTRACT_ACID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GROUP_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOMER_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FACILITY_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_CURRENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LOAN_BALANCE_FCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LOAN_BALANCE_LCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IENC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TOTAL_BALANCE_FCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TOTAL_BALANCE_LCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FX_RATE = table.Column<int>(type: "int", nullable: false),
                    INT_RATE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OVERDUE_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DATE_PAST_DUE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DAYS_PAST_DUE = table.Column<int>(type: "int", nullable: false),
                    DATE_GRANTED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXPIRY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TOD_LIMIT = table.Column<int>(type: "int", nullable: false),
                    SANCTION_LIMIT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OPERATIVE_ACCT_BALANCE_FCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OPERATIVE_ACCT_BALANCE_LCY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OPERATIVE_ACCOUNT_CURRENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUST_SECTOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUST_SECTOR_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUST_SUB_SECTOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUST_SUB_SECTOR_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TENOR_MONTH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TENOR_DAYS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INSTALMENT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_REPAYMENT_AMOUNT_DUE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NEWCUM_REPAYMENT_AMOUNT_DUE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_REPAYMENT_AMOUNT_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_INTEREST_REPAYMENT_AMOUNT_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_INTEREST_DUE_NOT_YET_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUM_PRINCIPAL_DUE_NOT_YET_PAID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PIENC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GL_ASSET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INT_COLL_BACID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCHM_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCT_CRNCY_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAST_CREDIT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_CREDIT_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NEXT_PRINCIPAL_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NEXT_INTEREST_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NEXT_REP_INT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FLOW_START_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NEXT_DMD_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PRIN_REPAYMENT_FREQUENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INT_REPAYMENT_FREQUENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL_COLLECTION_3M = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TOTAL_COLLECTION_6M = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TOTAL_COLLECTION_12M = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DETAILS_OF_SECURITIES_OTHERS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLLATERAL_DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLLATERAL_VALUE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SBU_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SBU_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOL_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRANCH_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BANK_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGION_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BM_STAFF_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOM_STAFF_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_OFFICER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BANK_HEAD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGION_HEAD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIRECTORATE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIRECTORATE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIRECTORATE_HEAD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RISKRATING = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERFORMANCE_CLASS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERFORMANCE_CLASS_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ROWNUMBER_ORDERBY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    RECONFIRMED = table.Column<bool>(type: "bit", nullable: false),
                    APPROVED = table.Column<bool>(type: "bit", nullable: false),
                    ISRESOLVED = table.Column<bool>(type: "bit", nullable: false),
                    DEADLINE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROCESSEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoanFileBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finacleDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_finacleDatas_LoanFileBatch_LoanFileBatchId",
                        column: x => x.LoanFileBatchId,
                        principalTable: "LoanFileBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_finacleDatas_customerDetails_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audits_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "financialCovenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CovenantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financialCovenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_financialCovenants_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanFileItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    IsFileVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsFileExitOnServer = table.Column<bool>(type: "bit", nullable: false),
                    CustomDocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequesterUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_LoanFileItem_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nonFinancialCovenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CovenantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nonFinancialCovenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nonFinancialCovenants_finacleDatas_AppId",
                        column: x => x.AppId,
                        principalTable: "finacleDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audits_AppId",
                table: "audits",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_audits_UserId",
                table: "audits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_discrepancyRemarks_AppId",
                table: "discrepancyRemarks",
                column: "AppId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_finacleDatas_CustomerId",
                table: "finacleDatas",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_finacleDatas_LoanFileBatchId",
                table: "finacleDatas",
                column: "LoanFileBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_financialCovenants_AppId",
                table: "financialCovenants",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileBatch_AppId",
                table: "LoanFileBatch",
                column: "AppId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileItem_AppId",
                table: "LoanFileItem",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileItem_FileBatchId",
                table: "LoanFileItem",
                column: "FileBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanFileItem_Region_ServerStatus",
                table: "LoanFileItem",
                columns: new[] { "Region", "IsFileExitOnServer" });

            migrationBuilder.CreateIndex(
                name: "IX_nonFinancialCovenants_AppId",
                table: "nonFinancialCovenants",
                column: "AppId");

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
                name: "audits");

            migrationBuilder.DropTable(
                name: "cat");

            migrationBuilder.DropTable(
                name: "discrepancyRemarks");

            migrationBuilder.DropTable(
                name: "financialCovenants");

            migrationBuilder.DropTable(
                name: "LoanFileItem");

            migrationBuilder.DropTable(
                name: "nonFinancialCovenants");

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
                name: "finacleDatas");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LoanFileBatch");

            migrationBuilder.DropTable(
                name: "customerDetails");
        }
    }
}
