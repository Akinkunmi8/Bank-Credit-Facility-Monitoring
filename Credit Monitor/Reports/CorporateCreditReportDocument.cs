using Credit_Monitor.ViewModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Data.Common;
using System.Linq;

namespace Credit_Monitor.Reports;

/// <summary>
/// Presentation only. The caller supplies the same approved-loan model used by ReportPreview.
/// </summary>
public sealed class CorporateCreditReportDocument : IDocument
{
    private readonly CreditMonitoringReportViewModel _model;
    private const string Navy = "#142B49";
    private const string Blue = "#1467D9";
    private const string Pale = "#F3F6FA";
    private const string Muted = "#64748B";
    private const string Green = "#16794C";
    private const string Border = "#DFE6EF";

    public CorporateCreditReportDocument(CreditMonitoringReportViewModel model)
        => _model = model ?? throw new ArgumentNullException(nameof(model));

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        var loan = _model.CreditProfile?.loan
            ?? throw new InvalidOperationException("Loan details are required.");
        if (!loan.approved)
            throw new InvalidOperationException("Only CMS-approved loans may be reported.");

        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.MarginHorizontal(34);
            page.MarginVertical(30);
            page.DefaultTextStyle(t => t.FontFamily("Lato").FontSize(9).FontColor(Navy));
            page.Header().Element(Header);
            page.Content().PaddingTop(16).Column(c =>
            {
                c.Spacing(16);
                c.Item().Element(ExecutiveSummary);
                c.Item().Element(CustomerDetails);
                c.Item().Element(LoanDetails);
                c.Item().Element(DocumentStatus);
                c.Item().Element(Covenants);
                c.Item().Element(Approval);
            });
            page.Footer().Element(Footer);
        });
    }

    private void Header(IContainer box)
    {
        box.Column(c =>
        {
            c.Item().Row(r =>
            {
                r.RelativeItem().Column(left =>
                {
                    left.Item().Text("CREDIT MONITOR").Bold().FontSize(11).FontColor(Blue);
                    left.Item().PaddingTop(4).Text("CREDIT MONITORING REVIEW REPORT")
                        .Bold().FontSize(16).FontColor(Navy);
                    left.Item().Text("Post-disbursement credit monitoring | Internal report")
                        .FontSize(8).FontColor(Muted);
                });
                r.ConstantItem(160).AlignRight().Column(right =>
                {
                    right.Item().Text("CMS APPROVED").Bold().FontSize(10).FontColor(Green);
                    right.Item().Text($"Issued {_model.GeneratedAt:dd MMM yyyy, HH:mm}")
                        .FontSize(8).FontColor(Muted);
                });
            });
            c.Item().PaddingTop(12).Height(3).Background(Blue);
        });
    }

    private void ExecutiveSummary(IContainer box)
    {
        var loan = _model.CreditProfile.loan!;
        box.Column(c =>
        {
            c.Item().Element(x => SectionTitle(x, "EXECUTIVE OVERVIEW", "01"));
            c.Item().Background(Pale).Padding(12).Column(summary =>
            {
                summary.Spacing(8);
                summary.Item().Element(x => Pair(x, "Application ID", loan.id.ToString()));
                summary.Item().Element(x => Pair(x, "Customer / Account", loan.accounT_NAME));
                summary.Item().Element(x => Pair(x, "Facility Type", loan.facilitY_TYPE));
                summary.Item().Element(x => Pair(x, "Branch / Region", $"{loan.brancH_NAME} / {loan.regioN_NAME}"));
                summary.Item().Element(x => Pair(x, "Review Status", "CMS Approved"));
            });
        });
    }

    private void CustomerDetails(IContainer box)
    {
        var p = _model.CreditProfile.customer;
        box.Column(c =>
        {
            c.Item().Element(x => SectionTitle(x, "CUSTOMER DETAILS", "02"));
            c.Item().Element(x => Pair(x, "Full Name", string.Join(" ",
                new[] { p?.tittle, p?.firstname, p?.middlename, p?.lastname }
                .Where(s => !string.IsNullOrWhiteSpace(s)))));
            c.Item().Element(x => Pair(x, "Gender / Nationality", $"{p?.gender} / {p?.nationallity}"));
            c.Item().Element(x => Pair(x, "Date of Birth", Convert.ToString(p?.dob)));
            c.Item().Element(x => Pair(x, "Marital Status", p?.maritalStatus));
            c.Item().Element(x => Pair(x, "Dependants", Convert.ToString(p?.numbOfDependant)));
        });
    }

    private void LoanDetails(IContainer box)
    {
        var loan = _model.CreditProfile.loan!;

        box.Column(column =>
        {
            column.Spacing(8);

            // SECTION HEADER
            column.Item().Element(x =>
                SectionTitle(x, "LOAN / FACILITY DETAILS", "03"));

            // BASIC FACILITY INFORMATION
            column.Item().Element(x =>
                Pair(x, "Account Name", loan.accounT_NAME));

            column.Item().Element(x =>
                Pair(x, "Facility Type", loan.facilitY_TYPE));

            // ACCOUNT & SECTOR IDENTIFIERS
            FinancialGroup(
                column,
                "ACCOUNT & SECTOR IDENTIFIERS",
                ("Account Number", loan.accounT_NUMBER),
                ("Contract ID / ACID",
                    $"{loan.contracT_ID} / {loan.contracT_ACID}"),
                ("Customer / Group ID",
                    $"{loan.customeR_ID} / {loan.grouP_ID}"),
                ("Scheme / GL Asset",
                    $"{loan.schM_CODE} / {loan.gL_ASSET}")
            );

            // FINANCIAL BALANCES & RATES
            FinancialGroup(
                column,
                "FINANCIAL BALANCES & RATES",
                ("Sanction / TOD Limit",
                    $"{loan.sanctioN_LIMIT:N2} / {loan.toD_LIMIT:N2}"),
                ("Loan Balance (LCY / FCY)",
                    $"{loan.loaN_BALANCE_LCY:N2} / {loan.loaN_BALANCE_FCY:N2}"),
                ("Operative Balance",
                    $"{loan.operativE_ACCT_BALANCE_LCY:N2}"),
                ("Interest / FX Rate",
                    $"{loan.inT_RATE}% / {loan.fX_RATE}")
            );

            // REPAYMENT & COLLECTION ANALYSIS
            FinancialGroup(
                column,
                "REPAYMENT & COLLECTION ANALYSIS",
                ("Instalment",
                    $"{loan.instalment:N2}"),
                ("Cumulative Paid",
                    $"{loan.cuM_PRINCIPAL_REPAYMENT_AMOUNT_PAID:N2}"),
                ("Due Not Paid",
                    $"{loan.cuM_PRINCIPAL_DUE_NOT_YET_PAID:N2}"),
                ("Overdue Amount",
                    $"{loan.overduE_AMT:N2}")
            );

            // RISK, PERFORMANCE & TIMELINE
            FinancialGroup(
                column,
                "RISK, PERFORMANCE & TIMELINE",
                ("Risk Rating", loan.riskrating),
                ("Performance Class", loan.performancE_CLASS_DESC),
                ("Past Due (Days)",
                    $"{loan.dayS_PAST_DUE}"),
                ("Expiry Date",
                    $"{loan.expirY_DATE:dd MMM yyyy}")
            );

            // BRANCH, REGION & OFFICERS
            FinancialGroup(
                column,
                "BRANCH, REGION & OFFICERS",
                ("Branch", loan.brancH_NAME),
                ("Region", loan.regioN_NAME),
                ("Account Officer", loan.accounT_OFFICER),
                ("BM / BOM",
                    $"{loan.bM_STAFF_NAME} / {loan.boM_STAFF_NAME}"),
                ("Directorate", loan.directoratE_NAME)
            );
        });
    }

    private void DocumentStatus(IContainer box)
    {
        box.Column(c =>
        {
            c.Item().Element(x => SectionTitle(x, "DOCUMENT VERIFICATION", "04"));
            var docs = _model.Documents;
            if (docs == null || docs.Count == 0)
            {
                c.Item().Text("No documents recorded.").Italic().FontColor(Muted);
                return;
            }
            c.Item().Table(t =>
            {
                t.ColumnsDefinition(cols => { cols.RelativeColumn(2); cols.RelativeColumn(); cols.RelativeColumn(1.4f); });
                t.Header(h =>
                {
                    h.Cell().Element(HeaderCell).Text("Document");
                    h.Cell().Element(HeaderCell).Text("Category");
                    h.Cell().Element(HeaderCell).Text("Verification");
                });
                foreach (var doc in docs)
                {
                    t.Cell().Element(DataCell).Text(doc.FileName ?? "—");
                    t.Cell().Element(DataCell).Text(Convert.ToString(doc.DocType) ?? "—");
                    t.Cell().Element(DataCell).Text(doc.IsVerified ? "Verified" : "Pending verification")
                        .FontColor(doc.IsVerified ? Green : "#B45309");
                }
            });
        });
    }

    private static void FinancialGroup(ColumnDescriptor column,string title,params (string Label, string? Value)[] fields)
    {
        // Group heading
        column.Item()
            .PaddingTop(12)
            .PaddingBottom(6)
            .Text(title)
            .Bold()
            .FontSize(10)
            .FontColor(Colors.Blue.Darken2);

        // Financial details table
        column.Item()
            .Border(0.5f)
            .BorderColor(Colors.Grey.Lighten2)
            .Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(3);
                });

                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i];

                    var background = i % 2 == 0
                        ? Colors.Grey.Lighten4
                        : Colors.White;

                    table.Cell()
                        .Background(background)
                        .Padding(8)
                        .Text(field.Label)
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken2);

                    table.Cell()
                        .Background(background)
                        .Padding(8)
                        .Text(string.IsNullOrWhiteSpace(field.Value)
                            ? "—"
                            : field.Value)
                        .Bold()
                        .FontSize(9)
                        .FontColor(Colors.Black);
                }
            });
    }
    private void Covenants(IContainer box)
    {
        box.Column(c =>
        {
            c.Item().Element(x => SectionTitle(x, "COVENANT TRACKER", "05"));
            c.Item().Text("FINANCIAL COVENANTS").Bold().FontColor(Navy);
            var financials = _model.Covenants?.Financials;
            if (financials?.Any() == true)
            {
                c.Item().Table(t =>
                {
                    t.ColumnsDefinition(cols => { cols.RelativeColumn(); cols.RelativeColumn(2); cols.RelativeColumn(); cols.RelativeColumn(); });
                    t.Header(h =>
                    {
                        h.Cell().Element(HeaderCell).Text("Type");
                        h.Cell().Element(HeaderCell).Text("Description");
                        h.Cell().Element(HeaderCell).Text("Target");
                        h.Cell().Element(HeaderCell).Text("Frequency");
                    });
                    foreach (var x in financials)
                    {
                        t.Cell().Element(DataCell).Text(Convert.ToString(x.CovenantType) ?? "—");
                        t.Cell().Element(DataCell).Text(x.Description ?? "—");
                        t.Cell().Element(DataCell).Text(Convert.ToString(x.TargetValue) ?? "—");
                        t.Cell().Element(DataCell).Text(Convert.ToString(x.Frequency) ?? "—");
                    }
                });
            }
            else c.Item().Text("No financial covenants recorded.").Italic().FontColor(Muted);

            c.Item().PaddingTop(10).Text("NON-FINANCIAL COVENANTS").Bold().FontColor(Navy);
            var nonFinancials = _model.Covenants?.NonFinancials;
            if (nonFinancials?.Any() == true)
            {
                c.Item().Table(t =>
                {
                    t.ColumnsDefinition(cols => { cols.RelativeColumn(); cols.RelativeColumn(2.5f); cols.RelativeColumn(); });
                    t.Header(h =>
                    {
                        h.Cell().Element(HeaderCell).Text("Type");
                        h.Cell().Element(HeaderCell).Text("Description");
                        h.Cell().Element(HeaderCell).Text("Frequency");
                    });
                    foreach (var x in nonFinancials)
                    {
                        t.Cell().Element(DataCell).Text(Convert.ToString(x.CovenantType) ?? "—");
                        t.Cell().Element(DataCell).Text(x.Description ?? "—");
                        t.Cell().Element(DataCell).Text(Convert.ToString(x.Frequency) ?? "—");
                    }
                });
            }
            else c.Item().Text("No non-financial covenants recorded.").Italic().FontColor(Muted);
        });
    }

    private void Approval(IContainer box)
    {
        box.Column(c =>
        {
            c.Item().Element(x => SectionTitle(x, "REVIEW & APPROVAL", "06"));
            c.Item().Background("#EAF6EE").Border(1).BorderColor("#B7DCC7").Padding(12)
                .Text("CMS APPROVED — This facility has completed the required credit monitoring approval workflow.")
                .Bold().FontColor(Green);
            c.Item().PaddingTop(8).Element(x => Pair(x, "Report Generated By", _model.GeneratedBy));
            c.Item().Element(x => Pair(x, "Generated On", _model.GeneratedAt.ToString("dd MMM yyyy HH:mm")));
        });
    }

    private void Footer(IContainer box)
    {
        box.BorderTop(1).BorderColor(Border).PaddingTop(8).Row(r =>
        {
            r.RelativeItem().Text("CREDIT MONITOR  •  CONFIDENTIAL — INTERNAL USE ONLY")
                .FontSize(7).FontColor(Muted);
            r.ConstantItem(100).AlignRight().Text(t =>
            {
                t.DefaultTextStyle(s => s.FontSize(7).FontColor(Muted));
                t.Span("Page "); t.CurrentPageNumber(); t.Span(" of "); t.TotalPages();
            });
        });
    }

    private static void SectionTitle(IContainer box, string label, string number)
        => box.PaddingBottom(7).BorderBottom(1).BorderColor(Border).Row(r =>
        {
            r.ConstantItem(25).Text(number).Bold().FontColor(Blue);
            r.RelativeItem().Text(label).Bold().FontSize(10).FontColor(Navy);
        });

    private static void Pair(IContainer box, string label, string? value)
        => box.PaddingVertical(5).Row(r =>
        {
            r.ConstantItem(160).Text(label.ToUpperInvariant()).FontSize(8).FontColor(Muted);
            r.RelativeItem().Text(string.IsNullOrWhiteSpace(value) ? "—" : value).Bold();
        });

    private static IContainer HeaderCell(IContainer box)
        => box.Background(Navy).Padding(7).DefaultTextStyle(x => x.FontColor(Colors.White).Bold().FontSize(8));

    private static IContainer DataCell(IContainer box)
        => box.BorderBottom(1).BorderColor(Border).Padding(7).DefaultTextStyle(x => x.FontSize(8));
}
