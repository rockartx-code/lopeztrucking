namespace InvoiceApp.Application.UseCases.Commands;

public sealed record UpdatePlaceFlags(
    Guid PlaceId,
    bool IsCompany,
    bool IsFrom,
    bool IsTo);
