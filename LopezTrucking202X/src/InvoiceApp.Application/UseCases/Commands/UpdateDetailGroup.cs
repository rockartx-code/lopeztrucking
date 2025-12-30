namespace InvoiceApp.Application.UseCases.Commands;

public sealed record UpdateDetailGroup(
    Guid DetailGroupId,
    string Description,
    decimal AmountBase,
    int EmptiesCount);
