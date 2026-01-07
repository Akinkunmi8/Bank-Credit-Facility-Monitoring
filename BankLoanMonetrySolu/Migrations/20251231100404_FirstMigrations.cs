using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLoanMonetrySolu.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigrations : Migration
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
                    interest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fees = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    DAYS_PAST_DUE = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    ROWNUMBER_ORDERBY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finacleDatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat");

            migrationBuilder.DropTable(
                name: "finacleDatas");
        }
    }
}
