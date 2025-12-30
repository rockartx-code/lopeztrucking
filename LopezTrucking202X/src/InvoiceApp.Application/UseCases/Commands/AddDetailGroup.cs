namespace InvoiceApp.Application.UseCases.Commands;

public sealed record AddDetailGroup(
    Guid InvoiceId,
    string Description,
    decimal AmountBase,
    int EmptiesCount);
