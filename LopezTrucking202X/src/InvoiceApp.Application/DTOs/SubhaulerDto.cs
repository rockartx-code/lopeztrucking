namespace InvoiceApp.Application.DTOs;

public sealed record SubhaulerDto(
    Guid Id,
    string Name,
    string? ContactName,
    string? Phone,
    int? LastInvoiceNo);
