namespace InvoiceApp.Application.UseCases.Commands;

public sealed record CreateInvoice(
    Guid CompanyId,
    string InvoiceNumber,
    DateOnly InvoiceDate);
