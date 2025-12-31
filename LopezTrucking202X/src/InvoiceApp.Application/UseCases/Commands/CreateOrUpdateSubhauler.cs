namespace InvoiceApp.Application.UseCases.Commands;

public sealed record CreateOrUpdateSubhauler(
    Guid Id,
    string Name,
    string? ContactName,
    string? Phone);
