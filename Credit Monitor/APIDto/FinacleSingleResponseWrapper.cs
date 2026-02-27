using Newtonsoft.Json;
namespace Credit_Monitor.APIDto
{
    public class FinacleSingleResponseWrapper
    {
        public bool isSuccess { get; set; }
        public string responseMessage { get; set; }

        // The main payload is under 'data' for this specific endpoint
        public FinacleLoanAndCustomerDto? data { get; set; }
        public object? dataList { get; set; }
        public object? detailsDtos { get; set; }
        public object? detailsDto { get; set; }
    }

    public class FinacleLoanAndCustomerDto
    {
        public Guid id { get; set; }
        public string country { get; set; }
        public string lE_BOOK { get; set; }
        public string yeaR_MONTH { get; set; }
        public string risK_ASSET_TYPE { get; set; }
        public string contracT_ID { get; set; }
        public string accounT_NUMBER { get; set; }
        public string contracT_ACID { get; set; }
        public string grouP_ID { get; set; }
        public string customeR_ID { get; set; }
        public string accounT_NAME { get; set; }
        public string facilitY_TYPE { get; set; }
        public string accounT_CURRENCY { get; set; }
        public decimal loaN_BALANCE_FCY { get; set; }
        public decimal loaN_BALANCE_LCY { get; set; }
        public decimal ienc { get; set; }
        public decimal totaL_BALANCE_FCY { get; set; }
        public decimal totaL_BALANCE_LCY { get; set; }
        public decimal fX_RATE { get; set; }
        public decimal inT_RATE { get; set; }
        public decimal? overduE_AMT { get; set; }
        public DateTime? datE_PAST_DUE { get; set; }
        public int? dayS_PAST_DUE { get; set; }
        public DateTime datE_GRANTED { get; set; }
        public DateTime transactioN_DATE { get; set; }
        public DateTime expirY_DATE { get; set; }
        public decimal toD_LIMIT { get; set; }
        public decimal sanctioN_LIMIT { get; set; }
        public decimal? operativE_ACCT_BALANCE_FCY { get; set; }
        public decimal operativE_ACCT_BALANCE_LCY { get; set; }
        public string operativE_ACCOUNT_CURRENCY { get; set; }
        public string cusT_SECTOR { get; set; }
        public string cusT_SECTOR_DESC { get; set; }
        public string cusT_SUB_SECTOR { get; set; }
        public string cusT_SUB_SECTOR_DESC { get; set; }
        public string tenoR_MONTH { get; set; }
        public string tenoR_DAYS { get; set; }
        public decimal instalment { get; set; }
        public decimal? cuM_REPAYMENT_AMOUNT_DUE { get; set; }
        public decimal newcuM_REPAYMENT_AMOUNT_DUE { get; set; }
        public decimal? cuM_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? cuM_PRINCIPAL_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? cuM_INTEREST_REPAYMENT_AMOUNT_PAID { get; set; }
        public decimal? cuM_INTEREST_DUE_NOT_YET_PAID { get; set; }
        public decimal? cuM_PRINCIPAL_DUE_NOT_YET_PAID { get; set; }
        public decimal pienc { get; set; }
        public string gL_ASSET { get; set; }
        public string inT_COLL_BACID { get; set; }
        public string schM_CODE { get; set; }
        public string accT_CRNCY_CODE { get; set; }
        public DateTime? lasT_CREDIT_DATE { get; set; }
        public decimal? lasT_CREDIT_AMT { get; set; }
        public decimal nexT_PRINCIPAL_AMOUNT { get; set; }
        public decimal nexT_INTEREST_AMOUNT { get; set; }
        public DateTime nexT_REP_INT_DATE { get; set; }
        public DateTime floW_START_DATE { get; set; }
        public DateTime nexT_DMD_DATE { get; set; }
        public string priN_REPAYMENT_FREQUENCY { get; set; }
        public string inT_REPAYMENT_FREQUENCY { get; set; }
        public decimal? totaL_COLLECTION_3M { get; set; }
        public decimal? totaL_COLLECTION_6M { get; set; }
        public decimal? totaL_COLLECTION_12M { get; set; }
        public string detailS_OF_SECURITIES_OTHERS { get; set; }
        public string collateraL_DESCRIPTION { get; set; }
        public string collateraL_VALUE { get; set; }
        public string sbU_NAME { get; set; }
        public string sbU_TYPE { get; set; }
        public string soL_ID { get; set; }
        public string branch { get; set; }
        public string brancH_NAME { get; set; }
        public string banK_NAME { get; set; }
        public string region { get; set; }
        public string regioN_NAME { get; set; }
        public string bM_STAFF_NAME { get; set; }
        public string boM_STAFF_NAME { get; set; }
        public string accounT_OFFICER { get; set; }
        public string banK_HEAD { get; set; }
        public string regioN_HEAD { get; set; }
        public string directorate { get; set; }
        public string directoratE_NAME { get; set; }
        public string directoratE_HEAD { get; set; }
        public string riskrating { get; set; }
        public string performancE_CLASS { get; set; }
        public string performancE_CLASS_DESC { get; set; }
        public string rownumbeR_ORDERBY { get; set; }
        public bool confirmed { get; set; }
        public bool reconfirmed { get; set; }
        public bool approved { get; set; }
        public bool isresolved { get; set; }
        public DateTime processedate { get; set; }

        public Guid customerId { get; set; }

        // Nested Customer Object
        public CustomerDetailsDto customer { get; set; }

        public Guid? LoanFileBatchId { get; set; }
    }
}


