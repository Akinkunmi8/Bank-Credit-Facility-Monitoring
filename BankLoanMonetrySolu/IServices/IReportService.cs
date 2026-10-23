using BankLoanMonetrySolu.DTO;

namespace BankLoanMonetrySolu.IServices
{
    public interface IReportService
    {
        Task<byte[]> GenerateFullReport(ReportRequestDto request);
        Task<FullDashboardReportData> GetFullReportData(ReportRequestDto request);
    }
}
