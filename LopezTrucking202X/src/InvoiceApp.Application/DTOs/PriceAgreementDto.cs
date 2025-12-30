using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.DTOs;

public sealed record PriceAgreementDto(
    Guid Id,
    Guid CompanyId,
    string MixName,
    DateOnly EffectiveDate,
    IReadOnlyList<PriceAgreementItemDto> Items);

public sealed record PriceAgreementItemDto(
    ItemType ItemType,
    decimal EmptyUnitPrice,
    decimal BaseRate);
