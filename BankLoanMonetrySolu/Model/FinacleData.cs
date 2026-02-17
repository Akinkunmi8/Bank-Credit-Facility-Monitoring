using System.ComponentModel.DataAnnotations.Schema;

namespace BankLoanMonetrySolu.Model
{
    public class FinacleData
    {
        public Guid Id { get; set; }
        public string? COUNTRY {  get; set; }
        public string? LE_BOOK { get; set; }
        public string? YEAR_MONTH { get; set; }
        public string? RISK_ASSET_TYPE { get; set; }
        public string? CONTRACT_ID { get; set; }
        public string? ACCOUNT_NUMBER { get; set; }
        public string? CONTRACT_ACID { get; set; }
        public string? GROUP_ID { get; set; }
        public string? CUSTOMER_ID {  get; set; }
        public string? ACCOUNT_NAME { get; set; }
        public string? FACILITY_TYPE { get; set; }
        public string? ACCOUNT_CURRENCY { get; set; }
        public decimal? LOAN_BALANCE_FCY { get; set; }
        public decimal? LOAN_BALANCE_LCY { get; set; }
        public decimal? IENC {  get; set; }
        public decimal? TOTAL_BALANCE_FCY { get; set; }
        public decimal? TOTAL_BALANCE_LCY { get; set; }
        public int FX_RATE { get; set; }
        public decimal? INT_RATE { get; set; }
        public decimal? OVERDUE_AMT { get; set; }
        public DateTime? DATE_PAST_DUE { get; set; }
        public int DAYS_PAST_DUE { get; set; }
        public DateTime? DATE_GRANTED {  get; set; }
        public DateTime? TRANSACTION_DATE { get; set; }
        public DateTime? EXPIRY_DATE { get; set; }
        public int TOD_LIMIT {  get; set; }
        public decimal? SANCTION_LIMIT { get; set; }
        public decimal? OPERATIVE_ACCT_BALANCE_FCY { get; set; }
        public decimal? OPERATIVE_ACCT_BALANCE_LCY { get; set; }
        public string? OPERATIVE_ACCOUNT_CURRENCY { get; set; }
        public string? CUST_SECTOR {  get; set; }
        public string? CUST_SECTOR_DESC { get; set; }
        public string? CUST_SUB_SECTOR { get; set; }
        public string? CUST_SUB_SECTOR_DESC { get; set; }
        public string? TENOR_MONTH { get; set; }
        public string? TENOR_DAYS {  get; set; }
        public decimal? INSTALMENT {  get; set; }
        public decimal? CUM_REPAYMENT_AMOUNT_DUE { get; set; }
        public decimal? NEWCUM_REPAYMENT_AMOUNT_DUE { get; set; }
        public decimal? CUM_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? CUM_INTEREST_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? CUM_INTEREST_DUE_NOT_YET_PAID { get; set; }
        public decimal? CUM_PRINCIPAL_DUE_NOT_YET_PAID { get; set; }
        public decimal? PIENC { get; set; }
        public string? GL_ASSET {  get; set; }
        public string? INT_COLL_BACID { get; set; }
        public string? SCHM_CODE { get; set; }
        public string? ACCT_CRNCY_CODE { get; set; }
        public DateTime? LAST_CREDIT_DATE { get; set; }
        public decimal? LAST_CREDIT_AMT { get; set; }
        public decimal? NEXT_PRINCIPAL_AMOUNT {  get; set; }
        public decimal? NEXT_INTEREST_AMOUNT { get; set; }
        public DateTime? NEXT_REP_INT_DATE { get; set; }
        public DateTime? FLOW_START_DATE { get; set; }
        public DateTime? NEXT_DMD_DATE { get; set; }
        public string? PRIN_REPAYMENT_FREQUENCY { get; set; }
        public string? INT_REPAYMENT_FREQUENCY { get; set; }
        public decimal? TOTAL_COLLECTION_3M { get; set; }
        public decimal? TOTAL_COLLECTION_6M { get; set; }
        public decimal? TOTAL_COLLECTION_12M { get; set; }
        public string? DETAILS_OF_SECURITIES_OTHERS { get; set; }
        public string? COLLATERAL_DESCRIPTION { get; set; }
        public string? COLLATERAL_VALUE { get; set; }
        public string? SBU_NAME { get; set; }
        public string? SBU_TYPE { get; set; }
        public string? SOL_ID { get; set; }
        public string? BRANCH { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? BANK_NAME { get; set; }
        public string? REGION { get; set; }
        public string? REGION_NAME { get; set; }
        public string? BM_STAFF_NAME { get; set; }
        public string? BOM_STAFF_NAME { get; set; }
        public string? ACCOUNT_OFFICER { get; set; }
        public string? BANK_HEAD { get; set; }
        public string? REGION_HEAD { get; set; }
        public string? DIRECTORATE { get; set; }
        public string? DIRECTORATE_NAME { get; set; }
        public string? DIRECTORATE_HEAD { get; set; }
        public string? RISKRATING { get; set; }
        public string? PERFORMANCE_CLASS { get; set; }
        public string? PERFORMANCE_CLASS_DESC { get; set; }
        public string? ROWNUMBER_ORDERBY { get; set; }
        public bool CONFIRMED { get; set; }
        public bool RECONFIRMED { get; set; }
        public bool APPROVED { get; set; }
        public bool ISRESOLVED { get; set; }
        public DateTime? DEADLINE { get; set; }
        public DateTime? PROCESSEDATE { get; set; }
        public Guid? CustomerId { get; set; }
        public CustomerDetails? Customer { get; set; }
        public Guid? LoanFileBatchId { get; set; }
        public LoanFileBatch? LoanFileBatch { get; set; }


    }
}
