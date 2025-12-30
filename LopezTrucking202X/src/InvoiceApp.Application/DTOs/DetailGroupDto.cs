namespace InvoiceApp.Application.DTOs;

public sealed record DetailGroupDto(
    Guid Id,
    string Description,
    decimal AmountBase,
    int EmptiesCount,
    decimal EmptyUnitPrice,
    decimal AmountTotal);
