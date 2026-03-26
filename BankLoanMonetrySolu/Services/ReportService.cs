using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;

namespace BankLoanMonetrySolu.Services
{
    public class ReportService : IReportService
    {
        private readonly IFinacleData _finacleService;
        private readonly ITAT _tatService;

        public ReportService(IFinacleData finacleService, ITAT tatService)
        {
            _finacleService = finacleService;
            _tatService = tatService;
        }

        public async Task<byte[]> GenerateFullReport(ReportRequestDto request)
        {
            var reportData = new FullDashboardReportData();

            // 1. Data Aggregation
            if (request.Role == "CMS" || request.Role == "CMO")
            {
                reportData.Unreviewed = await _finacleService.GetAllPendingConfirmationByRegion(request.Region);
                reportData.WithinTATUnreviewed = await _tatService.GetDataWithinTATForUnReview(request.Region);
                reportData.BeyondTATUnreviewed = await _tatService.GetDataBeyondTATForUnReview(request.Region);
                reportData.Reviewed = await _finacleService.GetAllConfirmedFinacleData(request.Region);
            }

            if (request.Role == "CMS" || request.Role == "CMO" || request.Role == "AO" || request.Role == "BO")
            {
                reportData.Resolved = await _finacleService.GetResolvedFinacleData(request.Region);
                reportData.UnResoved = await _finacleService.GetAllReConfirmedFinacleDataByRegion(request.Region);
                reportData.WithinTATUnresolve = await _tatService.GetDataWithinTATForUnResolved(request.Region);
                reportData.BeyondTATUnresolve = await _tatService.GetDataBeyondTATForUnResolved(request.Region);
            }

            if (request.Role == "CMS")
            {
                reportData.Approve = await _finacleService.GetAllAprovedFinacleData(request.Region);
                reportData.BeyondTATReview = await _tatService.GetBeyondTATForReviewedforApproval(request.Region);
            }

            // 2. Routing
            return request.Format.ToUpper() == "EXCEL"
                ? await ExportToExcel(reportData, request.Role)
                : await ExportToPdf(reportData, request.Role);
        }

        private async Task<byte[]> ExportToExcel(FullDashboardReportData data, string role)
        {
            using var workbook = new XLWorkbook();

            // 1. SUMMARY SHEET
            var summarySheet = workbook.Worksheets.Add("Summary Report");
            summarySheet.Cell(1, 1).Value = $"{role} Dashboard Summary";
            summarySheet.Cell(1, 1).Style.Font.Bold = true;
            summarySheet.Cell(1, 1).Style.Font.FontSize = 16;

            // Table Headers
            summarySheet.Cell(3, 1).Value = "Category";
            summarySheet.Cell(3, 2).Value = "Count";
            summarySheet.Range("A3:B3").Style.Fill.BackgroundColor = XLColor.LightGray;

            // Map your counts
            int row = 4;
            AddSummaryRow(summarySheet, ref row, "Unreviewed", data.Unreviewed?.DataList?.Count);
            AddSummaryRow(summarySheet, ref row, "Reviewed", data.Reviewed?.DataList?.Count);
            AddSummaryRow(summarySheet, ref row, "Unresolved", data.UnResoved?.DataList?.Count);
            AddSummaryRow(summarySheet, ref row, "Resolved", data.Resolved?.DataList?.Count);
            AddSummaryRow(summarySheet, ref row, "Approved", data.Approve?.DataList?.Count);
            AddSummaryRow(summarySheet, ref row, "Beyond TAT (Review)", Convert.ToInt32(data.BeyondTATReview?.TATCount));
            AddSummaryRow(summarySheet, ref row, "Beyond TAT (Unresolve)", data.BeyondTATUnresolve?.Count);
            AddSummaryRow(summarySheet, ref row, "Beyond TAT (Unreview)", data.BeyondTATUnreviewed?.Count);
            AddSummaryRow(summarySheet, ref row, "Within TAT (Unresolve)", data.WithinTATUnresolve?.Count);
            AddSummaryRow(summarySheet, ref row, "Within TAT (Unreview)", data.WithinTATUnreviewed?.Count);

            // 2. DETAILED SHEETS (Only add if data exists)
            if (data.Reviewed?.DataList?.Any() == true)
                AddDataSheet(workbook, "Reviewed Details", data.Reviewed.DataList);

            if (data.Resolved?.DataList?.Any() == true)
                AddDataSheet(workbook, "Resolved Details", data.Resolved.DataList);

            if (data.Approve?.DataList?.Any() == true)
                AddDataSheet(workbook, "Approved Details", data.Approve.DataList);

            // 3. TAT SHEETS (Highlighting Beyond TAT in Red)
            if (data.BeyondTATUnresolve?.Any() == true)
                AddDataSheet(workbook, "Beyond TAT Unresolved", data.BeyondTATUnresolve, isWarning: true);
            if (data.BeyondTATUnreviewed?.Any() == true)
                AddDataSheet(workbook, "Beyond TAT Unreview", data.BeyondTATUnreviewed, isWarning: true);
            //if (data.BeyondTATReview?.TATCount?.Any() == true)
            //{
            //    AddDataSheet(workbook, "Beyond TAT Review for Approve", data.BeyondTATReview, isWarning: true);
            //}
            if (data.WithinTATUnresolve?.Any() == true)
                AddDataSheet(workbook, "Within TAT Unresolved", data.WithinTATUnresolve, isWarning: true);
            if (data.WithinTATUnreviewed?.Any() == true)
                AddDataSheet(workbook, "Within TAT Unreview", data.WithinTATUnreviewed, isWarning: true);

            summarySheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        // Helper to add summary lines safely
        private void AddSummaryRow(IXLWorksheet sheet, ref int row, string label, int? count)
        {
            sheet.Cell(row, 1).Value = label;
            sheet.Cell(row, 2).Value = count ?? 0;
            row++;
        }

        // Helper to create detailed data sheets
        private void AddDataSheet(XLWorkbook workbook, string sheetName, List<FinacleDataDto> items, bool isWarning = false)
        {
            var sheet = workbook.Worksheets.Add(sheetName);

            // Add Headers
            sheet.Cell(1, 1).Value = "Account Number";
            sheet.Cell(1, 2).Value = "Customer Name";
            sheet.Cell(1, 3).Value = "Amount";
            sheet.Cell(1, 4).Value = "Region";
            sheet.Cell(1, 5).Value = "Date Created";

            var headerRange = sheet.Range("A1:E1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = isWarning ? XLColor.RedMunsell : XLColor.LightSkyBlue;

            int currentRow = 2;
            foreach (var item in items)
            {
                sheet.Cell(currentRow, 1).Value = item.ACCOUNT_NUMBER;
                sheet.Cell(currentRow, 2).Value = item.ACCOUNT_NAME;
                sheet.Cell(currentRow, 3).Value = item.CUM_PRINCIPAL_REPAYMENT_AMOUNT_PAID;
                sheet.Cell(currentRow, 4).Value = item.REGION;
                sheet.Cell(currentRow, 5).Value = item.PROCESSEDATE;

                // Format Amount as Currency
                sheet.Cell(currentRow, 3).Style.NumberFormat.Format = "#,##0.00";
                currentRow++;
            }
            sheet.Columns().AdjustToContents();
        }

        private async Task<byte[]> ExportToPdf(FullDashboardReportData data, string role)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(QuestPDF.Helpers.Fonts.Verdana));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("BANK CREDIT MONITORING").FontSize(18).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().Text($"{role} REPORT").FontSize(11).FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                        });
                        row.RelativeItem().AlignRight().Text(DateTime.Now.ToString("dd MMM yyyy"));
                    });

                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Text("Executive Summary").FontSize(14).SemiBold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Using explicit lambda to avoid method group errors
                            table.Cell().Element(c => PdfCellStyle(c)).Text("Metric").SemiBold();
                            table.Cell().Element(c => PdfCellStyle(c)).Text("Total").SemiBold();

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Unreviewed");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.Unreviewed?.Data?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Unresolved");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.UnResoved?.Data?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Approve");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.Approve?.Data?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Beyond TAT Unresolved");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.BeyondTATUnresolve?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Beyond TAT Unreveiw");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.BeyondTATUnreviewed?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Beyond TAT Review");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.BeyondTATReview?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Within TAT Unresolved");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.WithinTATUnresolve?.ToString() ?? "0");

                            table.Cell().Element(c => PdfCellStyle(c)).Text("Within TAT Unreview");
                            table.Cell().Element(c => PdfCellStyle(c)).Text(data.WithinTATUnreviewed?.ToString() ?? "0");
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }

        // Renamed and made explicit to avoid IContainer ambiguity
        private static QuestPDF.Infrastructure.IContainer PdfCellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                .PaddingVertical(5);
        }
    }
}