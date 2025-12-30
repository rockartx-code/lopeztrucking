namespace InvoiceApp.Application.DTOs;

public sealed record PlaceDto(
    Guid Id,
    string Name,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode);
