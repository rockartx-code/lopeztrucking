using System.Globalization;
using System.IO;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceApp.Infrastructure.Pdf;

public sealed class InvoicePdfGenerator : IPdfGenerator
{
    private const decimal DefaultAdvance = 0m;

    static InvoicePdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Task<byte[]> GenerateInvoiceAsync(InvoiceDto invoice, CancellationToken cancellationToken = default)
    {
        var totalBase = invoice.DetailGroups.Sum(group => group.AmountBase);
        var totalEmpties = invoice.DetailGroups.Sum(group => group.EmptiesCount * group.EmptyUnitPrice);
        var totalAdvance = DefaultAdvance;
        var total = invoice.DetailGroups.Sum(group => group.AmountTotal) - totalAdvance;

        var logoPath = ResolveLogoPath();
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Content().Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(header =>
                        {
                            header.Item().Text("Invoice").FontSize(20).SemiBold();
                            header.Item().Text($"Invoice #: {invoice.InvoiceNumber}");
                            header.Item().Text($"Date: {invoice.InvoiceDate:MMM dd, yyyy}");
                        });

                        row.ConstantItem(120).AlignRight().AlignMiddle().Element(logo =>
                        {
                            if (!string.IsNullOrWhiteSpace(logoPath))
                            {
                                logo.Height(60).Image(logoPath);
                            }
                            else
                            {
                                logo.Height(60).Width(120).Background(Colors.Grey.Lighten3)
                                    .AlignCenter().AlignMiddle().Text("LOGO");
                            }
                        });
                    });

                    column.Item().PaddingTop(15).Element(container =>
                    {
                        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(subhauler =>
                        {
                            subhauler.Item().Text("Subhauler").SemiBold();
                            subhauler.Item().Text($"Company ID: {invoice.CompanyId}");
                        });
                    });

                    column.Item().PaddingTop(15).Element(container =>
                    {
                        container.Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Description");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Base");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Empties");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Empty Unit");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Total");
                            });

                            foreach (var group in invoice.DetailGroups)
                            {
                                table.Cell().Element(BodyCell).Text(group.Description);
                                table.Cell().Element(BodyCell).AlignRight().Text(FormatCurrency(group.AmountBase));
                                table.Cell().Element(BodyCell).AlignRight().Text(group.EmptiesCount.ToString(CultureInfo.InvariantCulture));
                                table.Cell().Element(BodyCell).AlignRight().Text(FormatCurrency(group.EmptyUnitPrice));
                                table.Cell().Element(BodyCell).AlignRight().Text(FormatCurrency(group.AmountTotal));
                            }
                        });
                    });

                    column.Item().PaddingTop(20).AlignRight().Element(container =>
                    {
                        container.Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(140);
                            });

                            table.Cell().Element(TotalLabelCell).Text("Base");
                            table.Cell().Element(TotalValueCell).Text(FormatCurrency(totalBase));

                            table.Cell().Element(TotalLabelCell).Text("Empties");
                            table.Cell().Element(TotalValueCell).Text(FormatCurrency(totalEmpties));

                            table.Cell().Element(TotalLabelCell).Text("Advance");
                            table.Cell().Element(TotalValueCell).Text(FormatCurrency(totalAdvance));

                            table.Cell().Element(TotalLabelCell).Text("Total").SemiBold();
                            table.Cell().Element(TotalValueCell).Text(FormatCurrency(total)).SemiBold();
                        });
                    });
                });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return Task.FromResult(pdfBytes);
    }

    private static string FormatCurrency(decimal value) =>
        value.ToString("C", CultureInfo.CurrentCulture);

    private static IContainer HeaderCell(IContainer container) =>
        container.Background(Colors.Grey.Lighten3).Padding(6).DefaultTextStyle(x => x.SemiBold());

    private static IContainer BodyCell(IContainer container) =>
        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(6).PaddingHorizontal(4);

    private static IContainer TotalLabelCell(IContainer container) =>
        container.PaddingVertical(4).AlignRight();

    private static IContainer TotalValueCell(IContainer container) =>
        container.PaddingVertical(4).AlignRight();

    private static string? ResolveLogoPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var assetsPath = Path.Combine(baseDirectory, "Assets", "StoreLogo.png");
        if (File.Exists(assetsPath))
        {
            return assetsPath;
        }

        var fallbackPath = Path.Combine(baseDirectory, "StoreLogo.png");
        return File.Exists(fallbackPath) ? fallbackPath : null;
    }
}
