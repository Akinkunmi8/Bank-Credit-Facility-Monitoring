

using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.Model;
using ExcelDataReader;
using System.Data;

using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

using ExcelDataReader;

using ExcelDataReader;
using Microsoft.Data.SqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankLoanMonetrySolu.Logic
{
    public class ReadExcelFile
    {
        private readonly IConfiguration _config;

        public ReadExcelFile(IConfiguration config)
        {
            _config = config;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task<string> ImportAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "No file provided";

            var finacleList = new List<FinacleData>();

            using var stream = file.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            // 1️⃣ Read header row and build column map
            reader.Read();
            var columnMap = new Dictionary<string, int>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetValue(i)?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(name))
                    columnMap[name] = i;
            }
            var counter = 1;
            // 2️⃣ Read data rows
            while (reader.Read())
            {
                // Skip empty rows
                if (reader.FieldCount == 0) continue;

                var data = new FinacleData
                {
                    Id = Guid.NewGuid(),
                    COUNTRY = GetString(reader, columnMap, "COUNTRY"),
                    LE_BOOK = GetString(reader, columnMap, "LE_BOOK"),
                    YEAR_MONTH = GetString(reader, columnMap, "YEAR_MONTH"),
                    RISK_ASSET_TYPE = GetString(reader, columnMap, "RISK_ASSET_TYPE"),
                    CONTRACT_ID = GetString(reader, columnMap, "CONTRACT_ID"),
                    ACCOUNT_NUMBER = GetString(reader, columnMap, "ACCOUNT_NUMBER"),
                    CONTRACT_ACID = GetString(reader, columnMap, "CONTRACT_ACID"),
                    GROUP_ID = GetString(reader, columnMap, "GROUP_ID"),
                    CUSTOMER_ID = GetString(reader, columnMap, "CUSTOMER_ID"),
                    ACCOUNT_NAME = GetString(reader, columnMap, "ACCOUNT_NAME"),
                    FACILITY_TYPE = GetString(reader, columnMap, "FACILITY_TYPE"),
                    ACCOUNT_CURRENCY = GetString(reader, columnMap, "ACCOUNT_CURRENCY"),
                    LOAN_BALANCE_FCY = GetDecimal(reader, columnMap, "LOAN_BALANCE_FCY"),
                    LOAN_BALANCE_LCY = GetDecimal(reader, columnMap, "LOAN_BALANCE_LCY"),
                    IENC = GetDecimal(reader, columnMap, "IENC"),
                    TOTAL_BALANCE_FCY = GetDecimal(reader, columnMap, "TOTAL_BALANCE_FCY"),
                    TOTAL_BALANCE_LCY = GetDecimal(reader, columnMap, "TOTAL_BALANCE_LCY"),
                    FX_RATE = GetInt(reader, columnMap, "FX_RATE") ?? 0,
                    INT_RATE = GetDecimal(reader, columnMap, "INT_RATE"),
                    OVERDUE_AMT = GetDecimal(reader, columnMap, "OVERDUE_AMT"),
                    DATE_PAST_DUE = GetDate(reader, columnMap, "DATE_PAST_DUE"),
                    DAYS_PAST_DUE = GetDate(reader, columnMap, "DAYS_PAST_DUE"),
                    DATE_GRANTED = GetDate(reader, columnMap, "DATE_GRANTED"),
                    TRANSACTION_DATE = GetDate(reader, columnMap, "TRANSACTION_DATE"),
                    EXPIRY_DATE = GetDate(reader, columnMap, "EXPIRY_DATE"),
                    TOD_LIMIT = GetInt(reader, columnMap, "TOD_LIMIT") ?? 0,
                    SANCTION_LIMIT = GetDecimal(reader, columnMap, "SANCTION_LIMIT"),
                    OPERATIVE_ACCT_BALANCE_FCY = GetDecimal(reader, columnMap, "OPERATIVE_ACCT_BALANCE_FCY"),
                    OPERATIVE_ACCT_BALANCE_LCY = GetDecimal(reader, columnMap, "OPERATIVE_ACCT_BALANCE_LCY"),
                    OPERATIVE_ACCOUNT_CURRENCY = GetString(reader, columnMap, "OPERATIVE_ACCOUNT_CURRENCY"),
                    CUST_SECTOR = GetString(reader, columnMap, "CUST_SECTOR"),
                    CUST_SECTOR_DESC = GetString(reader, columnMap, "CUST_SECTOR_DESC"),
                    CUST_SUB_SECTOR = GetString(reader, columnMap, "CUST_SUB_SECTOR"),
                    CUST_SUB_SECTOR_DESC = GetString(reader, columnMap, "CUST_SUB_SECTOR_DESC"),
                    TENOR_MONTH = GetString(reader, columnMap, "TENOR_MONTH"),
                    TENOR_DAYS = GetString(reader, columnMap, "TENOR_DAYS"),
                    INSTALMENT = GetDecimal(reader, columnMap, "INSTALMENT"),
                    CUM_REPAYMENT_AMOUNT_DUE = GetDecimal(reader, columnMap, "CUM_REPAYMENT_AMOUNT_DUE"),
                    NEWCUM_REPAYMENT_AMOUNT_DUE = GetDecimal(reader, columnMap, "NEWCUM_REPAYMENT_AMOUNT_DUE"),
                    CUM_REPAYMENT_AMOUNT_PAID = GetDecimal(reader, columnMap, "CUM_REPAYMENT_AMOUNT_PAID"),
                    CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID = GetDecimal(reader, columnMap, "CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID"),
                    CUM_INTEREST_REPAYMENT_AMOUNT_PAID = GetDecimal(reader, columnMap, "CUM_INTEREST_REPAYMENT_AMOUNT_PAID"),
                    CUM_INTEREST_DUE_NOT_YET_PAID = GetDecimal(reader, columnMap, "CUM_INTEREST_DUE_NOT_YET_PAID"),
                    CUM_PRINCIPAL_DUE_NOT_YET_PAID = GetDecimal(reader, columnMap, "CUM_PRINCIPAL_DUE_NOT_YET_PAID"),
                    PIENC = GetDecimal(reader, columnMap, "PIENC"),
                    GL_ASSET = GetString(reader, columnMap, "GL_ASSET"),
                    INT_COLL_BACID = GetString(reader, columnMap, "INT_COLL_BACID"),
                    SCHM_CODE = GetString(reader, columnMap, "SCHM_CODE"),
                    ACCT_CRNCY_CODE = GetString(reader, columnMap, "ACCT_CRNCY_CODE"),
                    LAST_CREDIT_DATE = GetDate(reader, columnMap, "LAST_CREDIT_DATE"),
                    LAST_CREDIT_AMT = GetDecimal(reader, columnMap, "LAST_CREDIT_AMT"),
                    NEXT_PRINCIPAL_AMOUNT = GetDecimal(reader, columnMap, "NEXT_PRINCIPAL_AMOUNT"),
                    NEXT_INTEREST_AMOUNT = GetDecimal(reader, columnMap, "NEXT_INTEREST_AMOUNT"),
                    NEXT_REP_INT_DATE = GetDate(reader, columnMap, "NEXT_REP_INT_DATE"),
                    FLOW_START_DATE = GetDate(reader, columnMap, "FLOW_START_DATE"),
                    NEXT_DMD_DATE = GetDate(reader, columnMap, "NEXT_DMD_DATE"),
                    PRIN_REPAYMENT_FREQUENCY = GetString(reader, columnMap, "PRIN_REPAYMENT_FREQUENCY"),
                    INT_REPAYMENT_FREQUENCY = GetString(reader, columnMap, "INT_REPAYMENT_FREQUENCY"),
                    TOTAL_COLLECTION_3M = GetDecimal(reader, columnMap, "TOTAL_COLLECTION_3M"),
                    TOTAL_COLLECTION_6M = GetDecimal(reader, columnMap, "TOTAL_COLLECTION_6M"),
                    TOTAL_COLLECTION_12M = GetDecimal(reader, columnMap, "TOTAL_COLLECTION_12M"),
                    DETAILS_OF_SECURITIES_OTHERS = GetString(reader, columnMap, "DETAILS_OF_SECURITIES_OTHERS"),
                    COLLATERAL_DESCRIPTION = GetString(reader, columnMap, "COLLATERAL_DESCRIPTION"),
                    COLLATERAL_VALUE = GetString(reader, columnMap, "COLLATERAL_VALUE"),
                    SBU_NAME = GetString(reader, columnMap, "SBU_NAME"),
                    SBU_TYPE = GetString(reader, columnMap, "SBU_TYPE"),
                    SOL_ID = GetString(reader, columnMap, "SOL_ID"),
                    BRANCH_NAME = GetString(reader, columnMap, "BRANCH_NAME"),
                    BANK_NAME = GetString(reader, columnMap, "BANK_NAME"),
                    REGION_NAME = GetString(reader, columnMap, "REGION_NAME"),
                    BRANCH = GetString(reader, columnMap, "BRANCH"),
                    REGION = GetString(reader, columnMap, "REGION"),
                    BM_STAFF_NAME = GetString(reader, columnMap, "BM_STAFF_NAME"),
                    BOM_STAFF_NAME = GetString(reader, columnMap, "BOM_STAFF_NAME"),
                    ACCOUNT_OFFICER = GetString(reader, columnMap, "ACCOUNT_OFFICER"),
                    BANK_HEAD = GetString(reader, columnMap, "BANK_HEAD"),
                    REGION_HEAD = GetString(reader, columnMap, "REGION_HEAD"),
                    DIRECTORATE = GetString(reader, columnMap, "DIRECTORATE"),
                    DIRECTORATE_NAME = GetString(reader, columnMap, "DIRECTORATE_NAME"),
                    DIRECTORATE_HEAD = GetString(reader, columnMap, "DIRECTORATE_HEAD"),
                    RISKRATING = GetString(reader, columnMap, "RISKRATING"),
                    PERFORMANCE_CLASS = GetString(reader, columnMap, "PERFORMANCE_CLASS"),
                    PERFORMANCE_CLASS_DESC = GetString(reader, columnMap, "PERFORMANCE_CLASS_DESC"),
                    ROWNUMBER_ORDERBY = GetString(reader, columnMap, "ROWNUMBER_ORDERBY"),
                    APPROVED = true,                             
                    CONFIRMED = false,  
                    CustomerId = Guid.Parse("11111111-1111-1111-1111-11111111111" + counter),
                    RECONFIRMED = false,                          
                    PROCESSEDATE = DateTime.Now,                  
                };

                finacleList.Add(data);
                counter++;
            }

            // 3️⃣ Bulk insert
            await BulkInsertAsync(finacleList);

            return $"{finacleList.Count} records imported successfully";
        }

        private string? GetString(IExcelDataReader r, Dictionary<string, int> map, string c)
            => map.ContainsKey(c) ? r.GetValue(map[c])?.ToString() : null;

        private decimal? GetDecimal(IExcelDataReader r, Dictionary<string, int> map, string c)
            => map.ContainsKey(c) && decimal.TryParse(r.GetValue(map[c])?.ToString(), out var d) ? d : null;

        private int? GetInt(IExcelDataReader r, Dictionary<string, int> map, string c)
            => map.ContainsKey(c) && int.TryParse(r.GetValue(map[c])?.ToString(), out var i) ? i : null;

        
        private DateTime? GetDate(IExcelDataReader r, Dictionary<string, int> map, string c)
        {
            if (!map.ContainsKey(c)) return null;
            var v = r.GetValue(map[c]);
            if (v is DateTime dt) return dt;
            return DateTime.TryParse(v?.ToString(), out var d) ? d : null;
        }

        //private async Task BulkInsertAsync(List<FinacleData> list)
        //{
        //    if (list.Count == 0) return;

        //    var connStr = _config.GetConnectionString("DbConn");

        //    using var table = new DataTable();

        //    // Dynamically create DataTable schema from FinacleData properties
        //    var props = typeof(FinacleData).GetProperties();
        //    foreach (var prop in props)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

        //    // Populate rows
        //    foreach (var item in list)
        //    {
        //        var row = table.NewRow();
        //        foreach (var prop in props)
        //        {
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        }
        //        table.Rows.Add(row);
        //    }

        //    // Bulk copy
        //    using var conn = new SqlConnection(connStr);
        //    await conn.OpenAsync();

        //    using var bulk = new SqlBulkCopy(conn)
        //    {
        //        DestinationTableName = "finacleDatas",
        //        BatchSize = 5000
        //    };

        //    foreach (DataColumn column in table.Columns)
        //    {
        //        bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        //    }

        //    await bulk.WriteToServerAsync(table);
        //}
        private async Task BulkInsertAsync(List<FinacleData> list)
        {
            if (list == null || list.Count == 0) return;

            var connStr = _config.GetConnectionString("DbConn");
            using var table = new DataTable();

            // 1. Filter properties: Only take simple types (strings, decimals, ints, etc.)
            // This ignores navigation properties like 'Customer' which cause the error.
            var props = typeof(FinacleData).GetProperties()
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                .ToList();

            foreach (var prop in props)
            {
                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, propType);
            }

            // 2. Populate rows safely
            foreach (var item in list)
            {
                var row = table.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();

            using var bulk = new SqlBulkCopy(conn)
            {
                DestinationTableName = "finacleDatas",
                BatchSize = 5000,
                // Optional: Adding this helps if your table has an Identity column
                // Options = SqlBulkCopyOptions.Default 
            };

            // 3. Map only the columns we actually added to the DataTable
            foreach (DataColumn column in table.Columns)
            {
                bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }

            try
            {
                await bulk.WriteToServerAsync(table);
            }
            catch (Exception ex)
            {
                // This log will help you identify if a specific column is missing in the DB
                throw new Exception($"Bulk Insert failed. Check if SQL table 'finacleDatas' has all the properties defined in FinacleData. Error: {ex.Message}");
            }
        }
    }
}




