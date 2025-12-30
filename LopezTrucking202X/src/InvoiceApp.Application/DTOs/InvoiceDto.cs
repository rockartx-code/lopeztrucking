namespace InvoiceApp.Application.DTOs;

public sealed record InvoiceDto(
    Guid Id,
    string InvoiceNumber,
    DateOnly InvoiceDate,
    Guid CompanyId,
    IReadOnlyList<DetailGroupDto> DetailGroups);
