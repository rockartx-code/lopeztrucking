using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LopezTruck202X.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LopezTruck202X.Services;

public sealed class InvoicePdfService
{
    public Task<string> GenerateAsync(Invoice invoice, string outputDirectory)
    {
        var fileName = $"{invoice.Number}-{Sanitize(invoice.Customer.Name)}.pdf";
        var fullPath = Path.Combine(outputDirectory, fileName);

        var document = new InvoiceDocument(invoice);
        document.GeneratePdf(fullPath);

        return Task.FromResult(fullPath);
    }

    private static string Sanitize(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Concat(value.Select(ch => invalid.Contains(ch) ? '_' : ch));
    }

    private sealed class InvoiceDocument : IDocument
    {
        private readonly Invoice _invoice;

        public InvoiceDocument(Invoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(24);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text("Gracias por su preferencia");
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("LOPEZ TRUCKING").FontSize(20).Bold();
                    column.Item().Text("Invoice").FontSize(14).SemiBold();
                });

                row.ConstantItem(200).Column(column =>
                {
                    column.Item().Text($"Invoice #{_invoice.Number}").Bold();
                    column.Item().Text($"Date: {_invoice.Date:MM/dd/yyyy}");
                    column.Item().Text($"Check: {_invoice.CheckNumber}");
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PaddingTop(10).Element(ComposeCustomer);
                column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                column.Item().Element(ComposeTable);
                column.Item().PaddingTop(10).AlignRight().Element(ComposeTotals);
            });
        }

        private void ComposeCustomer(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Subhauler").Bold();
                column.Item().Text(_invoice.Customer.Name);
                column.Item().Text(_invoice.Customer.Address);
                column.Item().Text($"{_invoice.Customer.City}, {_invoice.Customer.State}");
                column.Item().Text(_invoice.Customer.Phone);
            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.ConstantColumn(70);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("Date");
                    header.Cell().Element(HeaderCell).Text("Company");
                    header.Cell().Element(HeaderCell).Text("From");
                    header.Cell().Element(HeaderCell).Text("To");
                    header.Cell().Element(HeaderCell).Text("Dispatch");
                    header.Cell().Element(HeaderCell).Text("Empties");
                    header.Cell().Element(HeaderCell).Text("F.B.");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                });

                foreach (var line in _invoice.Lines)
                {
                    table.Cell().Element(BodyCell).Text(line.Date.ToString("MM/dd/yyyy"));
                    table.Cell().Element(BodyCell).Text(line.Company);
                    table.Cell().Element(BodyCell).Text(line.From);
                    table.Cell().Element(BodyCell).Text(line.To);
                    table.Cell().Element(BodyCell).Text(line.Dispatch);
                    table.Cell().Element(BodyCell).Text(line.Empties);
                    table.Cell().Element(BodyCell).Text(line.Fb);
                    table.Cell().Element(BodyCell).AlignRight().Text(line.Amount.ToString("C"));
                }
            });
        }

        private void ComposeTotals(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text($"Subtotal: {_invoice.Subtotal:C}");
                column.Item().Text($"Advance: {_invoice.Advance:C}");
                column.Item().Text($"Total: {_invoice.Total:C}").Bold();
            });
        }

        private static IContainer HeaderCell(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold()).PaddingBottom(4).BorderBottom(1).BorderColor(Colors.Grey.Medium);
        }

        private static IContainer BodyCell(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(4);
        }
    }
}
